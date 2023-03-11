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
			Hole,
			Reset
		}

		private Settings _settings;

		private const float SettingsBoxWidth  = 100f;
		private const float SettingsBoxHeight = 120f;

		private readonly Color _heightRingColor = new(0, 1, 0, 1);
		private readonly Color _heightTileColor = new(0, 1, 0, .2f);

		private readonly Color _wallRingColor = new(0, 0, 1, 1);
		private readonly Color _wallTileColor = new(0, 0, 1, .2f);

		private readonly Color _holeRingColor = new(1, 0, 0, 1);
		private readonly Color _holeTileColor = new(1, 0, 0, .2f);

		private readonly Color _resetRingColor = new(1, 1, 0, 1);
		private readonly Color _resetTileColor = new(1, 1, 0, .2f);

		private const float HighlightRadiusScale = 0.1f;
		private const float HighlightRadiusMin   = 0.5f;
		private const float HighlightRadiusMax   = 5.0f;

		private const float MinimumHeightModEpsilon = 0.1f;

		private float _highlightRadius = HighlightRadiusMin;

		private float _raycastTileHeight;
		private float _editedTileHeight;

		private Option<Vector2> _center;

		private List<Vector2Int> _highlightedTiles;

		private bool _leftClick;

		private void OnEnable() {
			_controller = (RoomController) target;

			_highlightedTiles = new List<Vector2Int>();

			_settings = Settings.Height;
		}

		private void SetMeshes() {
			Mesh mesh = TerrainMesh.Generate(_controller.Room);

			_controller.GetComponent<MeshFilter>().sharedMesh   = mesh;
			_controller.GetComponent<MeshCollider>().sharedMesh = mesh;
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
						//SetCurrentHeight(Room.GetTileAt((int) _center.Value.x, (int) _center.Value.y).Height, true);
						_raycastTileHeight = Room.GetTileAt((int) _center.Value.x, (int) _center.Value.y).Height;
						_editedTileHeight  = _raycastTileHeight;
						break;
					case Settings.Wall:
						if (_leftClick) PaintWalls();
						break;
					case Settings.Hole:
						if (_leftClick) PaintHoles();
						break;
					case Settings.Reset:
						if (_leftClick) PaintReset();
						break;
				}
			} else {
				_center.Empty();
				_highlightedTiles.Clear();
			}
		}

		private Color GetRingColor() {
			return _settings switch {
				       Settings.Height => _heightRingColor,
				       Settings.Wall   => _wallRingColor,
				       Settings.Hole   => _holeRingColor,
				       Settings.Reset  => _resetRingColor,
				       _               => Color.magenta
			       };
		}

		private Color GetTileColor() {
			return _settings switch {
				       Settings.Height => _heightTileColor,
				       Settings.Wall   => _wallTileColor,
				       Settings.Hole   => _holeTileColor,
				       Settings.Reset  => _resetTileColor,
				       _               => Color.magenta
			       };
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
			Undo.RecordObject(Room, $"Painted walls of {Room.name}");

			foreach (Vector2Int tile in _highlightedTiles) {
				Room.SetTileAt(new Room.Tile(Room.TileFlags.Wall, 0f), tile.x, tile.y);
			}
			
			SetMeshes();

			EditorUtility.SetDirty(Room);
		}

		private void PaintHoles() {
			Undo.RecordObject(Room, $"Painted holes of {Room.name}");

			foreach (Vector2Int tile in _highlightedTiles) {
				Room.SetTileAt(new Room.Tile(Room.TileFlags.Pit, 0f), tile.x, tile.y);
			}
			
			SetMeshes();

			EditorUtility.SetDirty(Room);
		}

		private void PaintReset() {
			Undo.RecordObject(Room, $"Painted tile resets of {Room.name}");

			foreach (Vector2Int tile in _highlightedTiles) {
				Room.SetTileAt(new Room.Tile(Room.TileFlags.None, 0f), tile.x, tile.y);
			}
			
			SetMeshes();

			EditorUtility.SetDirty(Room);
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
					if (e.button == 0) _leftClick = true;
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
					Vector3 centerPos = new Vector3(tile.x + 0.5f,
					                                _controller.transform.position.y
					                              + Room.GetTileAt(tile.x, tile.y).Height, tile.y + 0.5f);
					Handles.DrawSolidDisc(centerPos, _controller.transform.up, 0.4f);
				}

				if (_center.Enabled) {
					Handles.color = GetRingColor();
					Vector3 centerPos = new Vector3(_center.Value.x, _controller.transform.position.y + _editedTileHeight,
					                                _center.Value.y);
					Handles.DrawWireDisc(centerPos, _controller.transform.up, _highlightRadius);

					if (e.shift && _settings == Settings.Height) {
						Handles.color = Handles.xAxisColor;
						/*SetCurrentHeight(Handles.ScaleSlider(
							                 _raycastTileHeight + 1, centerPos,
							                 _controller.transform.up,
							                 Quaternion.identity,
							                 HandleUtility.GetHandleSize(centerPos), 0
						                 )
						               - 1, false);*/
						_editedTileHeight = Handles.ScaleSlider(
							                    _editedTileHeight + 1, centerPos,
							                    _controller.transform.up,
							                    Quaternion.identity,
							                    HandleUtility.GetHandleSize(centerPos), 0
						                    )
						                  - 1;

						if (Mathf.Abs(_editedTileHeight - _raycastTileHeight) > MinimumHeightModEpsilon) {
							Undo.RecordObject(Room, $"Changed height values of {Room.name}");
							
							foreach (Vector2Int tile in _highlightedTiles) {
								Room.SetTileAt(new Room.Tile(Room.TileFlags.None, _editedTileHeight), tile.x, tile.y);
							}
							
							SetMeshes();
							
							EditorUtility.SetDirty(Room);
						}
					}
				}
			}
		}

		private void DrawGUI(EditorWindow window) {
			Handles.BeginGUI();

			const float buttonHeight = SettingsBoxHeight * .8f / 4;

			const float screenBottomOffset = 30f + 40f;

			GUI.Box(
				new Rect(0, window.position.height - SettingsBoxHeight - screenBottomOffset, SettingsBoxWidth,
				         SettingsBoxHeight),
				"Terrain Options");

			if (GUI.Button(
				    new Rect(0, window.position.height - buttonHeight * 4 - screenBottomOffset, SettingsBoxWidth,
				             buttonHeight), "Height - G")) { _settings = Settings.Height; }

			if (GUI.Button(
				    new Rect(0, window.position.height - buttonHeight * 3 - screenBottomOffset, SettingsBoxWidth,
				             buttonHeight), "Wall - B")) { _settings = Settings.Wall; }

			if (GUI.Button(
				    new Rect(0, window.position.height - buttonHeight * 2 - screenBottomOffset, SettingsBoxWidth,
				             buttonHeight), "Hole - R")) { _settings = Settings.Hole; }

			if (GUI.Button(
				    new Rect(0, window.position.height - buttonHeight * 1 - screenBottomOffset, SettingsBoxWidth,
				             buttonHeight), "Reset - Y")) { _settings = Settings.Reset; }
			
			if (GUI.Button(new Rect(0, window.position.height - 30 - 30, 100, 30),
			               "Reset Mesh")) { SetMeshes(); }

			Handles.EndGUI();
		}
	}
}