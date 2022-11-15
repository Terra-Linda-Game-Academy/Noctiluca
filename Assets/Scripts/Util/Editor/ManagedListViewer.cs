using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace Util.Editor {
	public class ManagedListViewer<T> : BindableElement, INotifyValueChanged<List<T>> {
		public new class UxmlFactory : UxmlFactory<ManagedListViewer<T>, UxmlTraits> { }

		public List<T> value { get; set; }

		private SerializedProperty _serializedList;

		private bool _testCheck;

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
		}

		public ManagedListViewer() { }

		public void SetValueWithoutNotify(List<T> newValue) { throw new System.NotImplementedException(); }
	}
}