using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace Util.Editor {
	[CustomEditor(typeof(ListTester))]
	public class ListTesterEditor : UnityEditor.Editor {
		private ListTester _listTester;

		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			SerializedProperty things = serializedObject.FindProperty("things");

			FunnyPopup popup = new FunnyPopup();
			ManagedListViewer<ListType> listViewer =
				new ManagedListViewer<ListType>(
					things, (() => { PopupWindow.Show(root.worldBound, popup); }, delegate { }));

			root.Add(listViewer);

			Button testButton = new Button(() => {
				                               FunnyPopup funnyPopup = new FunnyPopup();
				                               Debug.Log("popup obj created, showing");
				                               PopupWindow.Show(root.worldBound, funnyPopup);
				                               Debug.Log("awwagga");
			                               }) {text = "test"};
			root.Add(testButton);

			return root;
		}

		class FunnyPopup : PopupWindowContent {
			public Action OnComplete;

			public override Vector2 GetWindowSize() { return new Vector2(200, 100); }

			public override void OnGUI(Rect rect) { }

			public override void OnOpen() {
				Button closeButton = new Button(() => {
					                                Debug.Log("close");
					                                OnComplete.Invoke();
					                                editorWindow.Close();
				                                }) {text = "go away"};
				editorWindow.rootVisualElement.Add(closeButton);
			}
		}
	}
}