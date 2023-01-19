using UnityEditor;
using UnityEngine.UIElements;

namespace Util.Editor {
	[CustomEditor(typeof(ListTester))]
	public class ListTesterEditor : UnityEditor.Editor {
		private ListTester _listTester;

		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			SerializedProperty things = serializedObject.FindProperty("things");

			ManagedListViewer<ListType> listViewer =
				new ManagedListViewer<ListType>(things, ManagedListViewer<ListType>.Options.None);

			root.Add(listViewer);

			return root;
		}
	}
}