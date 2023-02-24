using UnityEditor;
using UnityEngine.UIElements;

namespace Levels.Editor {
	[CustomEditor(typeof(StupidRoomMeshGen))]
	public class StupidRoomMeshGenEditor : UnityEditor.Editor {
		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			StupidRoomMeshGen obj = target as StupidRoomMeshGen;
			
			root.Add(new Button(() => {obj.AdjustMesh();}) {text = "Mesh Func"} );
			
			return root;
		}
	}
}