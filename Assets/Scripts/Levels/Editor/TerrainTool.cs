using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Levels.Editor {
    [EditorTool("Terrain Tool", typeof(RoomController))]
    public class TerrainTool : EditorTool , IDrawSelectedHandles {
        public override GUIContent toolbarIcon => new GUIContent("Terrain Tool", "Use this to edit room terrain");
        
        public override void OnActivated() { base.OnActivated(); }
        public override void OnWillBeDeactivated() { base.OnWillBeDeactivated(); }

        public override void OnToolGUI(EditorWindow window) {
            
            
        }

        public void OnDrawHandles() {
            
        }
    }
}