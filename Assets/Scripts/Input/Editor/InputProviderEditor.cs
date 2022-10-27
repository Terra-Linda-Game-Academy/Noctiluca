using System;
using System.Linq;
using Input.ConcreteInputProviders;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace Input.Editor {
	[CustomEditor(typeof(InputProvider), true)]
	public class InputProviderEditor : UnityEditor.Editor {
		private TypeCache.TypeCollection _types;

		private SerializedProperty _middlewares;
		private VisualElement      _middlewareListContainer;

		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			VisualTreeAsset tree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Input/Editor/InputProviderEditor.uxml");
			tree.CloneTree(root);
			_middlewareListContainer = root.Q<VisualElement>("middleware-list-container");

			_middlewares = serializedObject.FindProperty("middlewares");
			PropertyField middlewaresField = new PropertyField(_middlewares) {style = {flexGrow = 1}};
			middlewaresField.Bind(serializedObject);
			_middlewareListContainer.Add(middlewaresField);

			/*for (int i = 0; i < middlewares.arraySize; i++) {
				SerializedProperty middleware = middlewares.GetArrayElementAtIndex(i);
				//Debug.Log(middleware.propertyType);
				PropertyField middlewareField = new PropertyField(middleware) {style = {flexGrow = 1}};
				middlewareField.Bind(middleware.serializedObject);
				//Debug.Log(middlewareField.binding);
				_middlewareListContainer.Add(middlewareField);
			}*/

			Type targetObjType = serializedObject.targetObject.GetType();

			if (targetObjType == typeof(PlayerInputProvider)) {
				_types = TypeCache.GetTypesDerivedFrom<PlayerInputMiddleware>();
			}

			Button debugButton = root.Q<Button>("debug-button");
			debugButton.clicked += () => { ((PlayerInputProvider) target).DebugPrint(); };

			Button debugAddButton = root.Q<Button>("debug-add-button");
			debugAddButton.clicked += () => {
				                          ((PlayerInputProvider) target).middlewares.Add(
					                          new PlayerInputSystemMiddleware());
			                          };

			/*switch (target) {
				case InputProvider<PlayerInputData> provider:
					provider.middlewares ??= new List<InputMiddleware<PlayerInputData>>();

					_types = TypeCache.GetTypesDerivedFrom<InputMiddleware<PlayerInputData>>();

					debugButton.clicked += provider.DebugPrint;

					ListView middlewareList = new ListView(provider.middlewares, 100f, 
					                                () => {
						                                       VisualElement newVE = new VisualElement();
						                                       _middlewareVE.CloneTree(newVE);
						                                       return newVE;
															},
													 (e, i) => {
						                                       InputMiddleware<PlayerInputData> item =
							                                       provider.middlewares[i];

						                                       VisualElement body = e.Q<VisualElement>("body");
						                                       body.Add(item.Body());
					                                       });
					_middlewareListContainer.Add(middlewareList);
					break;
			}*/

			Button addMiddlewareButton = root.Q<Button>("add-middleware-button");
			addMiddlewareButton.clicked += () => {
				                               AddMiddlewarePopup popup =
					                               new AddMiddlewarePopup(_types.Select(t => t.Name).ToArray());
				                               popup.OnDone += AddInputProvider;
				                               PopupWindow.Show(addMiddlewareButton.worldBound, popup);
			                               };

			return root;
		}

		private void AddInputProvider(int index) {
			Type pickedType = _types[index];

			var newMiddleware = Activator.CreateInstance(pickedType);
			
			_middlewares.arraySize++;
			_middlewares.GetArrayElementAtIndex(_middlewares.arraySize - 1).managedReferenceValue = newMiddleware;
			serializedObject.ApplyModifiedProperties();
			
			/*switch (target) {
				case InputProvider<PlayerInputData> provider:
					InputMiddleware<PlayerInputData> newMiddleware =
						(InputMiddleware<PlayerInputData>) Activator.CreateInstance(pickedType);

					//provider.middlewares.Add(newMiddleware);

					_middlewareListContainer.MarkDirtyRepaint();
					break;
			}*/
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