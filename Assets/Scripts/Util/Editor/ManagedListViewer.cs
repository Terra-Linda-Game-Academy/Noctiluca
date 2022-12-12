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

			this.Q<Button>("add-button").clicked    += () => { AddItem(addFunc.Invoke()); };
			this.Q<Button>("remove-button").clicked += () => { Shrink(); };

			Regenerate();
		}

		private void Regenerate() {
			_body.Clear();

			for (int i = 0; i < _serializedList.arraySize; i++) {
				SerializedProperty listItem = _serializedList.GetArrayElementAtIndex(i);

				HeadedPropertyField newField =
					new HeadedPropertyField(listItem, listItem.managedReferenceFieldTypename.Substring(16));

				_body.Add(newField);
			}
		}

		private void AddItem(T newItem) {
			_serializedList.arraySize++;
			_serializedList.GetArrayElementAtIndex(_serializedList.arraySize - 1).managedReferenceValue = newItem;
			_serializedList.serializedObject.ApplyModifiedProperties();

			Regenerate();
			_body.MarkDirtyRepaint();
		}

		private void Shrink() {
			_serializedList.arraySize--;
			_serializedList.serializedObject.ApplyModifiedProperties();

			Regenerate();
			_body.MarkDirtyRepaint();
		}

		class HeadedPropertyField : PropertyField {
			public HeadedPropertyField(SerializedProperty property, string label) : base(property, label) { }

			
		}
	}
}