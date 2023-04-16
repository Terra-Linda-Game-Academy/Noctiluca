using UnityEditor;
using UnityEngine.UIElements;
using Util.ConcretePools;
using Util.ConcreteWeightedItems;
using Util.Editor;

namespace Levels.Editor {
	[CustomEditor(typeof(RoomPool))]
	public class RoomPoolEditor : UnityEditor.Editor {
		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			SerializedProperty rooms = serializedObject.FindProperty("_items");
			ManagedListViewer<object> listViewer =
				new ManagedListViewer<object>(rooms, new []{typeof(WeightedRoom)}, ManagedListViewer<object>.Options.NoSize);
			
			root.Add(listViewer);

			return root;
		}
	}
}