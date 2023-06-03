using UnityEditor;
using UnityEngine.UIElements;

namespace UI.Editor {
	[CustomEditor(typeof(PotionDisplayController))]
	public class PotionDisplayControllerEditor : UnityEditor.Editor {
		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			root.Add(new IMGUIContainer(() => { DrawDefaultInspector(); }));

			root.Add(new Button(() => { (target as PotionDisplayController)?.Test(); }) {text  = "Test"});
			root.Add(new Button(() => { (target as PotionDisplayController)?.Empty(); }) {text = "Empty"});

			return root;
		}
	}
}