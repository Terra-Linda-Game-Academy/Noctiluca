using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Levels.Editor {
    [EditorTool("Room Tool", typeof(RoomController))]
    public class RoomTool : EditorTool, IDrawSelectedHandles {
        public override GUIContent toolbarIcon => new GUIContent("Room Tool", "Use this to edit room objects");

        private RoomController controller;
        private Room Room => controller.Room;

        public override void OnActivated() {
            controller = (RoomController) target;
        }

        public override void OnToolGUI(EditorWindow window) {
            if (Room is null) return;
            var so = new SerializedObject(Room);
            var size = so.FindProperty("size");
            var sizeVal = size.vector3IntValue;

            using (new Handles.DrawingScope(controller.transform.localToWorldMatrix)) {
                Handles.color = Handles.xAxisColor;
                var xHandlePos = new Vector3(sizeVal.x, 0, 0);
                sizeVal.x = Mathf.FloorToInt(Mathf.Max(1, Handles.ScaleSlider(
                    sizeVal.x, xHandlePos, Vector3.right,
                    Quaternion.identity, HandleUtility.GetHandleSize(xHandlePos), 1
                )));
                
                Handles.color = Handles.yAxisColor;
                var yHandlePos = new Vector3(0, sizeVal.y, 0);
                sizeVal.y = Mathf.FloorToInt(Mathf.Max(1, Handles.ScaleSlider(
                    sizeVal.y, yHandlePos, Vector3.up,
                    Quaternion.identity, HandleUtility.GetHandleSize(yHandlePos), 1
                )));

                Handles.color = Handles.zAxisColor;
                var zHandlePos = new Vector3(0, 0, sizeVal.z);
                sizeVal.z = Mathf.FloorToInt(Mathf.Max(1, Handles.ScaleSlider(
                    sizeVal.z, zHandlePos, Vector3.forward,
                    Quaternion.identity, HandleUtility.GetHandleSize(zHandlePos), 1
                )));
            }

            size.vector3IntValue = sizeVal;
            so.ApplyModifiedProperties();
            //todo: setup Editor3D and Property3D stuff, and call draw methods for each
        }
        

        public void OnDrawHandles() {
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
                
                Handles.color = Color.yellow;
                //todo: draw all preview things of room tiles
                for (int x = 1; x < Room.Size.x; x++) {
                    Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, 0, Room.Size.z), 1);
                }

                for (int z = 1; z < Room.Size.z; z++) {
                    Handles.DrawLine(new Vector3(0, 0, z), new Vector3(Room.Size.x, 0, z), 1);
                }
            }
        }
    }
}