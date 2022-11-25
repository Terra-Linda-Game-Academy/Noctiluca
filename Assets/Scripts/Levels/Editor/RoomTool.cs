using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEngine;

namespace Levels.Editor {
    [EditorTool("Room Tool", typeof(RoomController))]
    public class RoomTool : EditorTool , IDrawSelectedHandles {
        public override GUIContent toolbarIcon => new GUIContent("Room Tool", "Use this to edit room objects");

        private RoomController controller;
        private Room Room => controller.Room;
        private bool initted = false;

        private void Init() {
            if (!initted) {
                controller = (RoomController) target;
                initted = true;
            }
        }

        public override void OnToolGUI(EditorWindow window) {
            Init();
            if (Room is null) return;
            var so = new SerializedObject(Room);
            var size = so.FindProperty("size");
            var sizeVal = size.vector3IntValue;

            using (new Handles.DrawingScope(controller.transform.localToWorldMatrix)) {
                Handles.color = Handles.xAxisColor;
                sizeVal.x = Mathf.FloorToInt(Handles.ScaleSlider(
                    sizeVal.x, new Vector3(sizeVal.x, 0, 0), Vector3.right,
                    Quaternion.identity, 2, 1
                ));
                Handles.color = Handles.yAxisColor;
                sizeVal.y = Mathf.FloorToInt(Handles.ScaleSlider(
                    sizeVal.y, new Vector3(0, sizeVal.y, 0), Vector3.up,
                    Quaternion.identity, 2, 1
                ));
                Handles.color = Handles.zAxisColor;
                sizeVal.z = Mathf.FloorToInt(Handles.ScaleSlider(
                    sizeVal.z, new Vector3(0, 0, sizeVal.z), Vector3.forward,
                    Quaternion.identity, 2, 1
                ));
            }

            size.vector3IntValue = sizeVal;
            so.ApplyModifiedProperties(); 
            //todo: setup Editor3D and Property3D stuff, and call draw methods for each
        }

        public void OnDrawHandles() {
            Init();
            using (new Handles.DrawingScope(Color.white, controller.transform.localToWorldMatrix)) {
                if (Room is null) {
                    using (new Handles.DrawingScope(Color.red)) {
                        Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);
                        Handles.DrawWireCube(half, Vector3.one);
                        Handles.DrawWireCube(half, half);
                    }
                    return;
                }
                Handles.DrawWireCube((Vector3) Room.Size / 2.0f, Room.Size);
                //todo: draw all preview things of room tiles
            }
        }
    }
}