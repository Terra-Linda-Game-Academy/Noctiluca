using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Util.Editor {
	public class ManagedListViewer<T> : VisualElement where T : class {
		//TODO: won't show up in UIBuilder cuz we've giving it a constructor w/ parameters
		//public new class UxmlFactory : UxmlFactory<ManagedListViewer<T>, UxmlTraits> { }

		private readonly SerializedProperty _serializedList;

		private readonly VisualElement _body;

		/*private bool _testCheck;

		public new class UxmlTraits : BindableElement.UxmlTraits {
			private UxmlBoolAttributeDescription _testCheck = new() {name = "testCheck"};

			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription {
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext ctx) {
				base.Init(ve, bag, ctx);
				ManagedListViewer<T> listViewer = (ManagedListViewer<T>) ve;
				listViewer._testCheck = _testCheck.GetValueFromBag(bag, ctx);
			}
		}*/

		public ManagedListViewer(SerializedProperty list, Func<T> addFunc) {
			_serializedList = list;

			VisualTreeAsset tree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Util/Editor/ManagedListViewer.uxml");
			tree.CloneTree(this);

			this.Q<Foldout>("header").text = _serializedList.name;

			_body = this.Q<VisualElement>("body");

			this.Q<Button>("add-button").clicked += () => { AddItem(addFunc.Invoke()); };

			Regenerate();
		}

		private void Regenerate() {
			_body.Clear();

			for (int i = 0; i < _serializedList.arraySize; i++) {
				SerializedProperty listItem = _serializedList.GetArrayElementAtIndex(i);

				int localI = i;

				Action[] actions = {
					                   () => { ShiftUp(localI); },
					                   () => { ShiftDown(localI); },
					                   () => { RemoveItem(localI); }
				                   };

				HeadedFoldout foldout = new HeadedFoldout(listItem.managedReferenceFieldTypename.Substring(16),
				                                          actions);

				PropertyField propertyField = new PropertyField(listItem, "Data");

				foldout.Add(propertyField);

				_body.Add(foldout);
			}
		}

		private void AddItem(T newItem) {
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
			public HeadedFoldout(string labelText, Action[] actions) {
				VisualElement header = new VisualElement {style = {flexDirection = FlexDirection.Row, flexGrow = 1}};

				Label label = new Label(labelText);

				VisualElement buttonContainer =
					new VisualElement {style = {flexDirection = FlexDirection.Row, flexGrow = 1}};

				Button upButton   = new Button(actions[0]) {text = "^"};
				Button downButton = new Button(actions[1]) {text = "V"};

				Button removeButton = new Button(actions[2]) {text = "  X"};

				buttonContainer.Add(upButton);
				buttonContainer.Add(downButton);

				VisualElement buttonSpacer = new VisualElement {style = {flexGrow = 1}};
				buttonContainer.Add(buttonSpacer);

				buttonContainer.Add(removeButton);

				header.Add(label);
				header.Add(buttonContainer);

				VisualElement target = this.Query<VisualElement>(className: "unity-base-field__input").First();
				target.Add(header);
			}
		}
	}
}