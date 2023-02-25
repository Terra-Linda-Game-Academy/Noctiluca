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

		private readonly Color _highlightRingColor = new(0, 1, 0, 1);
		private readonly Color _highlightTileColor = new(0, 1, 0, 0.2f);

		private const float HighlightRadiusScale = 0.1f;
		private const float HighlightRadiusMin   = 0.5f;
		private const float HighlightRadiusMax   = 5.0f;

		private float _highlightRadius = HighlightRadiusMin;

		private float _currentHeight;

		private Option<Vector2> _center;

		private List<Vector2Int> _highlightedTiles;

		private void OnEnable() {
			_controller = (RoomController) target;

			_highlightedTiles = new List<Vector2Int>();
		}

		public override void OnToolGUI(EditorWindow window) {
			if (Room is null) return;
			Event e = Event.current;

			Raycast(e, window);

			CheckInputs(e);

			DrawHandles(e);
		}

		private void Raycast(Event e, EditorWindow window) {
			Vector2 mousePos = e.mousePosition;
			Ray     ray      = HandleUtility.GUIPointToWorldRay(mousePos);

			if (e.shift) return;

			if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 100, layerMask: LayerMask.GetMask("Room"))) {
				if (hit.transform != _controller.transform) return;

				window.Repaint(); //todo: this might be very inefficient/break stuff

				Vector3 localPos = _controller.transform.InverseTransformPoint(hit.point);

				_center.Value = new Vector2(localPos.x, localPos.z);

				_currentHeight = 0; //todo: replace w/ getting actual tile height (doesn't work cuz no room tile rn)
				//_currentHeight = Room.GetTileAt((int) _center.Value.x, (int) _center.Value.y).Height;

				CalculateHighlightedTiles();
			} else {
				_center.Empty();
				_highlightedTiles.Clear();
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

		private void CheckInputs(Event e) {
			if (!e.shift) return;

			switch (e.type) {
				case EventType.ScrollWheel:
					_highlightRadius -= e.delta.x * HighlightRadiusScale;
					_highlightRadius =  Mathf.Clamp(_highlightRadius, HighlightRadiusMin, HighlightRadiusMax);
					CalculateHighlightedTiles();
					break;
			}
		}

		private void DrawHandles(Event e) {
			using (new Handles.DrawingScope(_controller.transform.localToWorldMatrix)) {
				Handles.color = _highlightTileColor;
				foreach (Vector2Int tile in _highlightedTiles) {
					Vector3 centerPos = new Vector3(tile.x + 0.5f, _controller.transform.position.y, tile.y + 0.5f);
					Handles.DrawSolidDisc(centerPos, _controller.transform.up, 0.4f);
				}

				if (_center.Enabled) {
					Handles.color = _highlightRingColor;
					Vector3 centerPos = new Vector3(_center.Value.x, _controller.transform.position.y, _center.Value.y);
					Handles.DrawWireDisc(centerPos, _controller.transform.up, _highlightRadius);

					if (e.shift) {
						Handles.color = Handles.xAxisColor;
						Vector3 handlePos = centerPos + Vector3.up * _currentHeight;
						_currentHeight = Mathf.FloorToInt(Handles.ScaleSlider(
							                                  _currentHeight + 1, handlePos,
							                                  _controller.transform.up,
							                                  Quaternion.identity,
							                                  HandleUtility.GetHandleSize(handlePos), 1
						                                  )
						                                - 1);
					}
				}
			}
		}
	}
}