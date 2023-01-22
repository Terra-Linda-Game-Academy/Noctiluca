using System;
using System.Linq;
using UnityEditor;
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
				new ManagedListViewer<object>(_middlewares, _types, ManagedListViewer<object>.Options.NoSize);
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
}