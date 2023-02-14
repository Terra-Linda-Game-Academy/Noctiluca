using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Editor {
	[CustomEditor(typeof(DialogueController))]
	public class DialogueEditor : UnityEditor.Editor {
		public override VisualElement CreateInspectorGUI() {
			return new NodeEditor {style = { flexGrow = 1}};
		}

		class NodeEditor : GraphView {
			public NodeEditor() {
				
			}
		}
	}
}