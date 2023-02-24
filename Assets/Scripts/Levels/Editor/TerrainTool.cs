using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Levels.Editor {
	[EditorTool("Terrain Tool", typeof(RoomController))]
	public class TerrainTool : EditorTool {
		public override GUIContent toolbarIcon => new GUIContent("Terrain Tool", "Use this to edit room terrain");

		private RoomController _controller;
		private Room           Room => _controller.Room;

		private void OnEnable() { _controller = (RoomController) target; }

		public override void OnToolGUI(EditorWindow window) {
			if (Room is null) return;
			SerializedObject   so      = new SerializedObject(Room);
			SerializedProperty size    = so.FindProperty("size");
			Vector3Int         sizeVal = size.vector3IntValue;

			Vector2 mousePos = Event.current.mousePosition;
			Ray     ray      = HandleUtility.GUIPointToWorldRay(mousePos);

			if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 100, layerMask: LayerMask.GetMask("Room"))) {
				if (hit.transform != _controller.transform) return;
				
				Handles.color = Color.green;
				Handles.DrawWireDisc(hit.point, hit.normal, 1.0f);
				window.Repaint();

				Vector3    localPos    = _controller.transform.InverseTransformPoint(hit.point);
				Vector2Int localCoords = new Vector2Int((int) localPos.x, (int) localPos.z);
				Debug.Log(localCoords);
			}
		}
	}
}