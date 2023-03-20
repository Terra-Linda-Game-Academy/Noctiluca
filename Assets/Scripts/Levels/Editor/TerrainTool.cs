using System.Collections;
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

		private bool _leftClick;
		private bool _shiftLastFrame;
		private bool _drawHeightHandle;

		struct Selection {
			public BitArray TileMask;
			public float    StartingHeight;
		}

		private Selection _currentSelection;

		private void OnEnable() {
			_controller = (RoomController) target;

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

			if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 100, layerMask: LayerMask.GetMask("Room"))) {
				if (hit.transform != _controller.transform) return;

				window.Repaint(); //todo: this might be very inefficient/break stuff

				Vector3 localPos = _controller.transform.InverseTransformPoint(hit.point);

				_center.Value = new Vector2(localPos.x, localPos.z);
				switch (_settings) {
					case Settings.Height:
						_raycastTileHeight = Room.GetTileAt((int) _center.Value.x, (int) _center.Value.y).Height;
						break;
					case Settings.Wall:
						if (_leftClick) Paint(Room.TileFlags.Wall, 0);
						break;
					case Settings.Hole:
						if (_leftClick) Paint(Room.TileFlags.Pit, 0);
						break;
					case Settings.Reset:
						if (_leftClick) Paint(Room.TileFlags.None, 0);
						break;
				}
			} else { _center.Empty(); }
		}

		private void Paint(Room.TileFlags flags, float height) {
			List<Vector2Int> changedTiles = new List<Vector2Int>();
			bool             changes      = false;

			for (int i = 0; i < _currentSelection.TileMask.Length; i++) {
				if (_currentSelection.TileMask[i]
				 && (Room.tileMap[i].flags != flags || Room.tileMap[i].flags == Room.TileFlags.None)) {
					changedTiles.Add(Room.FromLinearIndex(i));
					changes = true;
				}
			}

			if (changes) {
				Undo.RecordObject(Room, $"Painted walls on {Room.name}");

				foreach (Vector2Int tile in changedTiles) {
					Room.SetTileAt(new Room.Tile(flags, height), tile.x, tile.y);
				}

				SetMeshes();

				EditorUtility.SetDirty(Room);
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

		private void CheckInputs(Event e) {
			if (e.shift) {
				switch (e.type) {
					case EventType.ScrollWheel:
						_highlightRadius -= e.delta.x * HighlightRadiusScale;
						_highlightRadius =  Mathf.Clamp(_highlightRadius, HighlightRadiusMin, HighlightRadiusMax);
						break;
					case EventType.MouseDown:
						if (e.button == 0) _leftClick = true;
						break;
					case EventType.MouseUp:
						if (e.button == 0) _leftClick = false;
						break;
				}

				if (!_shiftLastFrame) {
					_currentSelection = new Selection {
						                                  TileMask       = new BitArray(Room.tileMap.Length),
						                                  StartingHeight = _raycastTileHeight
					                                  };
					_drawHeightHandle = false;
				}

				if (_currentSelection.TileMask == null) return;

				switch (_settings) {
					case Settings.Height:
						if (_leftClick) {
							foreach (int i in GetHighlightedTileIndices()) { _currentSelection.TileMask[i] = true; }
						}

						break;
					default:
						_currentSelection.TileMask.SetAll(false);
						foreach (int i in GetHighlightedTileIndices()) { _currentSelection.TileMask[i] = true; }

						break;
				}
			} else if (_shiftLastFrame && _currentSelection.TileMask != null) {
				switch (_settings) {
					case Settings.Height:
						_editedTileHeight = _currentSelection.StartingHeight;
						_drawHeightHandle = true;
						break;
					default:
						_currentSelection.TileMask.SetAll(false);
						break;
				}
			}

			_shiftLastFrame = e.shift;
		}

		private List<int> GetHighlightedTileIndices() {
			List<int> indices = new List<int>();

			if (!_center.Enabled) return indices;

			Vector2Int localCoords = new Vector2Int((int) _center.Value.x, (int) _center.Value.y);

			int tileSearchDist = Mathf.CeilToInt(_highlightRadius);

			for (int i = localCoords.x - tileSearchDist; i <= localCoords.x + tileSearchDist; i++) {
				for (int j = localCoords.y - tileSearchDist; j <= localCoords.y + tileSearchDist; j++) {
					if (i < 0 || j < 0 || i >= Room.Size.x || j >= Room.Size.z) continue;

					float dx   = _center.Value.x - (i + 0.5f);
					float dy   = _center.Value.y - (j + 0.5f);
					float dist = Mathf.Sqrt(dx * dx + dy * dy);

					if (dist <= _highlightRadius) { indices.Add(Room.ToLinearIndex(i, j)); }
				}
			}

			return indices;
		}

		private void DrawHandles(Event e) {
			using (new Handles.DrawingScope(_controller.transform.localToWorldMatrix)) {
				Handles.color = GetTileColor();

				List<Vector2Int> tiles = new List<Vector2Int>();

				if (_currentSelection.TileMask != null) {
					for (int i = 0; i < _currentSelection.TileMask.Length; i++) {
						if (_currentSelection.TileMask[i]) {
							Vector2Int tile = Room.FromLinearIndex(i);
							tiles.Add(tile);
							Vector3 centerPos = new Vector3(tile.x + 0.5f,
							                                _controller.transform.position.y
							                              + Room.GetTileAt(tile.x, tile.y).Height, tile.y + 0.5f);
							Handles.DrawSolidDisc(centerPos, _controller.transform.up, 0.4f);
						}
					}
				}

				if (_center.Enabled) {
					Handles.color = GetRingColor();

					float x = 0;
					float z = 0;
					foreach (Vector2Int tile in tiles) {
						x += tile.x;
						z += tile.y;
					}

					x /= tiles.Count;
					z /= tiles.Count;

					Vector3 roomPos        = _controller.transform.position;
					Vector3 tileAveragePos = new Vector3(x, roomPos.y + _editedTileHeight, z);

					if (e.shift) {
						Vector3 discPos = new Vector3(_center.Value.x,
						                              roomPos.y + _raycastTileHeight,
						                              _center.Value.y);
						Handles.DrawWireDisc(discPos, _controller.transform.up, _highlightRadius);
					}

					if (_drawHeightHandle) {
						Handles.color = Handles.xAxisColor;
						_editedTileHeight = Handles.ScaleSlider(
							                    _editedTileHeight + 1, tileAveragePos,
							                    _controller.transform.up,
							                    Quaternion.identity,
							                    HandleUtility.GetHandleSize(tileAveragePos), 0
						                    )
						                  - 1;

						if (Mathf.Abs(_editedTileHeight - _raycastTileHeight) > MinimumHeightModEpsilon) {
							Undo.RecordObject(Room, $"Changed height values of {Room.name}");

							foreach (Vector2Int tile in tiles) {
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