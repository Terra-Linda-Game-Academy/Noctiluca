using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using Util;

namespace Levels.Editor {
	[EditorTool("Terrain Tool", typeof(RoomController))]
	public class TerrainTool : EditorTool {
		public override GUIContent toolbarIcon => new GUIContent("Terrain Tool", "Use this to edit room terrain");

		private RoomController _controller;
		private Room           Room => _controller.Room;

		enum Settings {
			Height,
			Wall,
			Hole
		}

		private Settings _settings;

		private const float SettingsBoxWidth  = 100f;
		private const float SettingsBoxHeight = 100f;

		private readonly Color _heightRingColor = new(0, 1, 0, 1);
		private readonly Color _heightTileColor = new(0, 1, 0, .2f);

		private readonly Color _wallRingColor = new(0, 0, 1, 1);
		private readonly Color _wallTileColor = new(0, 0, 1, .2f);

		private readonly Color _holeRingColor = new(1, 0, 0, 1);
		private readonly Color _holeTileColor = new(1, 0, 0, .2f);

		private const float HighlightRadiusScale = 0.1f;
		private const float HighlightRadiusMin   = 0.5f;
		private const float HighlightRadiusMax   = 5.0f;

		private float _highlightRadius = HighlightRadiusMin;

		private float _currentHeight;

		private Option<Vector2> _center;

		private List<Vector2Int> _highlightedTiles;

		private bool _leftClick;

		private void OnEnable() {
			_controller = (RoomController) target;

			_highlightedTiles = new List<Vector2Int>();

			_settings = Settings.Height;
		}

		public override void OnToolGUI(EditorWindow window) {
			if (Room is null) return;
			Event e = Event.current;

			//honestly no idea what this actually does under the hood, found it in a forum post
			//but it fixed the issue of getting left click mouse events so idk
			//https://forum.unity.com/threads/hurr-eventtype-mouseup-not-working-on-left-clicks.99909/
			//comment from 'Amitloaf', a fair way down the thread
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

			Raycast(e, window);

			CheckInputs(e);

			DrawHandles(e);

			DrawGUI(window);
		}

		private void Raycast(Event e, EditorWindow window) {
			Vector2 mousePos = e.mousePosition;
			Ray     ray      = HandleUtility.GUIPointToWorldRay(mousePos);

			if (e.shift && _settings == Settings.Height) return;

			if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 100, layerMask: LayerMask.GetMask("Room"))) {
				if (hit.transform != _controller.transform) return;

				window.Repaint(); //todo: this might be very inefficient/break stuff

				Vector3 localPos = _controller.transform.InverseTransformPoint(hit.point);

				_center.Value = new Vector2(localPos.x, localPos.z);

				CalculateHighlightedTiles();

				switch (_settings) {
					case Settings.Height:
						_currentHeight = 0; //todo: replace w/ getting actual tile height (doesn't work cuz no room tile rn)
						//_currentHeight = Room.GetTileAt((int) _center.Value.x, (int) _center.Value.y).Height;
						break;
					case Settings.Wall:
						if (_leftClick) PaintWalls();
						break;
					case Settings.Hole:
						if (_leftClick) PaintHoles();
						break;
				}
				
				
			} else {
				_center.Empty();
				_highlightedTiles.Clear();
			}
		}

		private Color GetRingColor() {
			switch (_settings) {
				case Settings.Height: return _heightRingColor;
				case Settings.Wall:   return _wallRingColor;
				case Settings.Hole:   return _holeRingColor;
				default:              return Color.magenta;
			}
		}

		private Color GetTileColor() {
			switch (_settings) {
				case Settings.Height: return _heightTileColor;
				case Settings.Wall:   return _wallTileColor;
				case Settings.Hole:   return _holeTileColor;
				default:              return Color.magenta;
			}
		}

		private void CalculateHighlightedTiles() {
			Vector2Int localCoords = new Vector2Int((int) _center.Value.x, (int) _center.Value.y);

			_highlightedTiles.Clear();
			int tileSearchDist = Mathf.CeilToInt(_highlightRadius);

			for (int i = localCoords.x - tileSearchDist; i <= localCoords.x + tileSearchDist; i++) {
				for (int j = localCoords.y - tileSearchDist; j <= localCoords.y + tileSearchDist; j++) {
					if (i < 0 || j < 0 || i >= Room.Size.x || j >= Room.Size.z) continue;

					float dx   = _center.Value.x - (i + 0.5f);
					float dy   = _center.Value.y - (j + 0.5f);
					float dist = Mathf.Sqrt(dx * dx + dy * dy);

					if (dist <= _highlightRadius) _highlightedTiles.Add(new Vector2Int(i, j));
				}
			}
		}

		private void PaintWalls() {
			foreach (Vector2Int tile in _highlightedTiles) {
				Debug.Log("painting walls"); //todo: actually set walls, but tiles don't exist yet (& also aren't writable??)
				//Room.GetTileAt(tile.x, tile.y) = new Room.Tile(Room.TileFlags.Wall, 0);
			}
		}

		private void PaintHoles() {
			foreach (Vector2Int tile in _highlightedTiles) {
				Debug.Log("painting holes"); //todo: actually set holes, but tiles don't exist yet (& also aren't writable??)
				//Room.GetTileAt(tile.x, tile.y) = new Room.Tile(Room.TileFlags.Pit, 0);
			}
		}

		private void CheckInputs(Event e) {
			if (!e.shift) return;

			switch (e.type) {
				case EventType.ScrollWheel:
					_highlightRadius -= e.delta.x * HighlightRadiusScale;
					_highlightRadius =  Mathf.Clamp(_highlightRadius, HighlightRadiusMin, HighlightRadiusMax);
					CalculateHighlightedTiles();
					break;
				case EventType.MouseDown:
					if (e.button  == 0) _leftClick = true;
					break;
				case EventType.MouseUp:
					if (e.button == 0) _leftClick = false;
					break;
			}
		}

		private void DrawHandles(Event e) {
			using (new Handles.DrawingScope(_controller.transform.localToWorldMatrix)) {
				Handles.color = GetTileColor();
				foreach (Vector2Int tile in _highlightedTiles) {
					Vector3 centerPos = new Vector3(tile.x + 0.5f, _controller.transform.position.y, tile.y + 0.5f);
					Handles.DrawSolidDisc(centerPos, _controller.transform.up, 0.4f);
				}

				if (_center.Enabled) {
					Handles.color = GetRingColor();
					Vector3 centerPos = new Vector3(_center.Value.x, _controller.transform.position.y, _center.Value.y);
					Handles.DrawWireDisc(centerPos, _controller.transform.up, _highlightRadius);

					if (e.shift && _settings == Settings.Height) {
						Handles.color = Handles.xAxisColor;
						Vector3 handlePos = centerPos + Vector3.up * _currentHeight;
						_currentHeight = Handles.ScaleSlider(
							                 _currentHeight + 1, handlePos,
							                 _controller.transform.up,
							                 Quaternion.identity,
							                 HandleUtility.GetHandleSize(handlePos), 0
						                 )
						               - 1;
					}
				}
			}
		}

		private void DrawGUI(EditorWindow window) {
			Handles.BeginGUI();

			const float buttonHeight = SettingsBoxHeight * .8f / 3;

			const float screenBottomOffset = 30f;

			GUI.Box(
				new Rect(0, window.position.height - SettingsBoxHeight - screenBottomOffset, SettingsBoxWidth,
				         SettingsBoxHeight),
				"Terrain Options");

			if (GUI.Button(
				    new Rect(0, window.position.height - buttonHeight * 3 - screenBottomOffset, SettingsBoxWidth,
				             buttonHeight), "Height - G")) { _settings = Settings.Height; }

			if (GUI.Button(
				    new Rect(0, window.position.height - buttonHeight * 2 - screenBottomOffset, SettingsBoxWidth,
				             buttonHeight), "Wall - B")) { _settings = Settings.Wall; }

			if (GUI.Button(
				    new Rect(0, window.position.height - buttonHeight * 1 - screenBottomOffset, SettingsBoxWidth,
				             buttonHeight), "Hole - R")) { _settings = Settings.Hole; }

			Handles.EndGUI();
		}
	}
}