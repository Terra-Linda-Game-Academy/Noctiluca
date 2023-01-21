using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Input.ConcreteInputProviders;
using Input.ConcreteInputProviders.Enemy;
using Input.Data;
using Input.Data.Enemy;
using Input.Events;
using Input.Events.Enemy;
using Input.Middleware;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Util.Editor;

namespace Input.Editor {
	[CustomEditor(typeof(InputProvider<,,,>), true)]
	public class InputProviderEditor : UnityEditor.Editor {
		private Type[] _types;

		private SerializedProperty _middlewares;
		private VisualElement      _middlewareListContainer;

		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			VisualTreeAsset tree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Input/Editor/InputProviderEditor.uxml");
			tree.CloneTree(root);
			_middlewareListContainer = root.Q<VisualElement>("middleware-list-container");
			
			_types = ((IBaseInputProvider) target).GetValidMiddlewareTypes().ToArray();

			_middlewares = serializedObject.FindProperty("_middlewares");

			ManagedListViewer<object> managedListViewer =
				new ManagedListViewer<object>(_middlewares, _types, ManagedListViewer<object>.Options.None);
			_middlewareListContainer.Add(managedListViewer);

			Button debugTypesButton = new Button(() => {
				                                     foreach (Type type in ((IBaseInputProvider) target)
				                                             .GetValidMiddlewareTypes()) {
					                                     Debug.Log(type);
				                                     }
			                                     }) {text = "type debug"};
			root.Add(debugTypesButton);

			return root;
		}
	}

	public class AddMiddlewarePopup : PopupWindowContent {
		public Action<int> OnDone;

		private readonly string[] _typeNames;

		public AddMiddlewarePopup(string[] typeNames) { _typeNames = typeNames; }

		public override Vector2 GetWindowSize() { return new Vector2(200, 100); }

		public override void OnGUI(Rect rect) { }

		public override void OnOpen() {
			VisualTreeAsset tree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Input/Editor/AddMiddlewarePopup.uxml");
			tree.CloneTree(editorWindow.rootVisualElement);

			ScrollView scrollView = editorWindow.rootVisualElement.Q<ScrollView>();

			for (int i = 0; i < _typeNames.Length; i++) {
				int i1 = i;

				Button newButton = new Button(() => {
					                              OnDone?.Invoke(i1);
					                              editorWindow.Close();
				                              }) {text = _typeNames[i]};

				scrollView.Add(newButton);
			}
		}
	}
}