using UnityEditor;
using UnityEngine.UIElements;
using Util.Editor;

namespace Levels.Editor {
	[CustomEditor(typeof(RoomPool))]
	public class RoomPoolEditor : UnityEditor.Editor {
		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			SerializedProperty rooms = serializedObject.FindProperty("rooms");
			ManagedListViewer<RoomPool.WrappedRoom> listViewer =
				new ManagedListViewer<RoomPool.WrappedRoom>(rooms, new []{typeof(Room)}, ManagedListViewer<RoomPool.WrappedRoom>.Options.NoSize);
			
			root.Add(listViewer);

			return root;
		}
	}
}