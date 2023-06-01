using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace Util.Editor {
	public class ManagedListViewer<T> : VisualElement where T : new() {
		private SerializedProperty _serializedList;

		private VisualElement _body;
		private VisualElement _listSizeContainer;

		private readonly Options _options;

		[Flags]
		public enum Options {
			None         = 0,
			ListSize     = 1,
			AddButton    = 2,
			RemoveButton = 4,
			MoveButtons  = 8,
			NoSize       = AddButton | RemoveButton | MoveButtons,
			Default      = ListSize  | AddButton    | RemoveButton | MoveButtons
		}

		private void Init(SerializedProperty list, Action addButtonEvt, string listName) {
			_serializedList = list;

			VisualTreeAsset tree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Util/Editor/ManagedListViewer.uxml");
			tree.CloneTree(this);

			this.Q<Foldout>("header").text = listName == "" ? ObjectNames.NicifyVariableName(_serializedList.name) : listName;

			_body              = this.Q<VisualElement>("body");
			_listSizeContainer = new VisualElement {style = {flexGrow = 1}};
			ElementAt(0).Q<VisualElement>(className: "unity-base-field__input").Add(_listSizeContainer);

			if ((_options & Options.AddButton) != 0) {
				VisualElement addButtonContainer = this.Q<VisualElement>("add-button-container");

				Button addButton = new Button(addButtonEvt) {text = "Add"};

				addButtonContainer.Add(addButton);
			}

			Regenerate();
		}

		public ManagedListViewer(SerializedProperty list, Options options = Options.Default, string name = "") {
			_options = options;
			Init(list, () => { AddItem(Activator.CreateInstance(typeof(T))); }, name);
		}

		public ManagedListViewer(SerializedProperty list, Type[] creationTypes, Options options = Options.Default, string name = "") {
			_options = options;
			if (creationTypes.Length == 1) {
				Init(list, () => { AddItem(Activator.CreateInstance(creationTypes[0])); }, name);
			} else {
				Init(list, () => {
					           AddItemPopup addItemPopup =
						           new AddItemPopup(creationTypes.Select(t => t.Name).ToArray());
					           addItemPopup.OnSelect += i => { AddItem(Activator.CreateInstance(creationTypes[i])); };
					           PopupWindow.Show(worldBound, addItemPopup);
				           }, name);
			}
		}

		private void Regenerate() {
			_body.Clear();

			if ((_options & Options.ListSize) != 0) {
				_listSizeContainer.Clear();

				_listSizeContainer.Add(new TextField {
					                                     value      = $"{_serializedList.arraySize}",
					                                     isReadOnly = true,
					                                     style      = {paddingLeft = 10}
				                                     });
			}

			for (int i = 0; i < _serializedList.arraySize; i++) {
				SerializedProperty listItem = _serializedList.GetArrayElementAtIndex(i);

				int localI = i;

				Action[] actions = {
					                   () => { ShiftUp(localI); },
					                   () => { ShiftDown(localI); },
					                   () => { RemoveItem(localI); }
				                   };

				string fullTypeName =
					ObjectNames.NicifyVariableName(
						listItem.managedReferenceFullTypename.Split('.').Last());

				HeadedFoldout foldout = new HeadedFoldout(fullTypeName,
				                                          actions,
				                                          (_options & Options.RemoveButton) != 0,
				                                          (_options & Options.MoveButtons)  != 0);

				PropertyField propertyField = new PropertyField(listItem, "Data");

				foldout.Add(propertyField);

				_body.Add(foldout);
			}

			_body.Bind(_serializedList.serializedObject);
		}

		private void AddItem(object newItem) {
			_serializedList.arraySize++;
			_serializedList.GetArrayElementAtIndex(_serializedList.arraySize - 1).managedReferenceValue = newItem;
			_serializedList.serializedObject.ApplyModifiedProperties();

			Regenerate();
		}

		private void RemoveItem(int index) {
			_serializedList.GetArrayElementAtIndex(index).managedReferenceValue = null;

			for (int i = index + 1; i < _serializedList.arraySize; i++) {
				_serializedList.GetArrayElementAtIndex(i - 1).managedReferenceValue =
					_serializedList.GetArrayElementAtIndex(i).managedReferenceValue;
			}

			_serializedList.arraySize--;

			_serializedList.serializedObject.ApplyModifiedProperties();

			Regenerate();
		}

		private void ShiftUp(int index) {
			if (index <= 0) return;

			object storage = _serializedList.GetArrayElementAtIndex(index - 1).managedReferenceValue;
			_serializedList.GetArrayElementAtIndex(index - 1).managedReferenceValue =
				_serializedList.GetArrayElementAtIndex(index).managedReferenceValue;
			_serializedList.GetArrayElementAtIndex(index).managedReferenceValue = storage;

			_serializedList.serializedObject.ApplyModifiedProperties();

			Regenerate();
		}

		private void ShiftDown(int index) {
			if (index >= _serializedList.arraySize - 1) return;

			object storage = _serializedList.GetArrayElementAtIndex(index + 1).managedReferenceValue;
			_serializedList.GetArrayElementAtIndex(index + 1).managedReferenceValue =
				_serializedList.GetArrayElementAtIndex(index).managedReferenceValue;
			_serializedList.GetArrayElementAtIndex(index).managedReferenceValue = storage;

			_serializedList.serializedObject.ApplyModifiedProperties();

			Regenerate();
		}

		class HeadedFoldout : Foldout {
			public HeadedFoldout(string labelText, Action[] actions, bool useRemove, bool useMove) {
				VisualElement header = new VisualElement {style = {flexDirection = FlexDirection.Row, flexGrow = 1}};

				Label label = new Label(labelText);

				VisualElement buttonContainer =
					new VisualElement {style = {flexDirection = FlexDirection.Row, flexGrow = 1}};

				if (useMove) {
					Button upButton   = new Button(actions[0]) {text = " ↑"};
					Button downButton = new Button(actions[1]) {text = " ↓"};

					buttonContainer.Add(upButton);
					buttonContainer.Add(downButton);
				}

				VisualElement buttonSpacer = new VisualElement {style = {flexGrow = 1}};
				buttonContainer.Add(buttonSpacer);

				if (useRemove) {
					Button removeButton = new Button(actions[2]) {text = "  X"};
					buttonContainer.Add(removeButton);
				}

				header.Add(label);
				header.Add(buttonContainer);

				VisualElement target = this.Query<VisualElement>(className: "unity-base-field__input").First();
				target.Add(header);
			}
		}

		class AddItemPopup : PopupWindowContent {
			public Action<int> OnSelect;

			private readonly string[] _options;

			public AddItemPopup(string[] options) { _options = options; }

			public override void OnGUI(Rect rect) { }

			public override Vector2 GetWindowSize() { return new Vector2(200, 100); }

			public override void OnOpen() {
				for (int i = 0; i < _options.Length; i++) {
					var localI = i;
					editorWindow.rootVisualElement.Add(new Button(() => {
						                                              OnSelect.Invoke(localI);
						                                              editorWindow.Close();
					                                              }) {text = _options[localI]});
				}
			}
		}
	}
}