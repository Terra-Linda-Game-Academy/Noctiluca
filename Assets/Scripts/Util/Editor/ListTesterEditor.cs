using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Util.Editor {
	[CustomEditor(typeof(ListTester))]
	public class ListTesterEditor : UnityEditor.Editor {
		private ListTester _listTester;

		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			SerializedProperty things = serializedObject.FindProperty("things");

			ManagedListViewer<ListType> listViewer =
				new ManagedListViewer<ListType>(things, new Task<ListType>(() => {
					                                                           Task.Delay(500).Wait();
					                                                           Debug.Log("post");
					                                                           return new ListType();
				                                                           }));

			root.Add(listViewer);

			return root;
		}
	}
}