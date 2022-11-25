using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Levels.Editor {
    [EditorTool("Room Tool", typeof(RoomController))]
    public class RoomTool : EditorTool , IDrawSelectedHandles {
        public override GUIContent toolbarIcon => new GUIContent("Room Tool", "Use this to edit room objects");
        
        public override void OnActivated() { base.OnActivated(); }
        public override void OnWillBeDeactivated() { base.OnWillBeDeactivated(); }

        public override void OnToolGUI(EditorWindow window) {
            //todo: setup Editor3D and Property3D stuff, and call draw methods for each
        }

        public void OnDrawHandles() {
            RoomController roomTarget = (RoomController) target;
            using (new Handles.DrawingScope(roomTarget.transform.localToWorldMatrix)) {
                if (!roomTarget.HasTarget) {
                    using (new Handles.DrawingScope(Color.red)) {
                        Vector3 half = new Vector3(0.5f, 0.5f, 0.5f);
                        Handles.DrawWireCube(half, Vector3.one);
                        Handles.DrawWireCube(half, half);
                    }
                    return;
                }
            }

            Handles.color = Color.white;
            Handles.DrawWireCube(Vector3.one, Vector3.one);
            
            //todo: draw all preview things of room tiles
        }
    }
}