using UnityEditor;
using UnityEngine.UIElements;

namespace Levels.Editor {
	[CustomEditor(typeof(LevelController))]
	public class LevelControllerEditor : UnityEditor.Editor {
		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new VisualElement();

			var generateButton = new Button();
			generateButton.text    =  "Generate Level";
			generateButton.clicked += () => { (target as LevelController).Generate(); };
			root.Add(generateButton);

			var defaultInspector = new IMGUIContainer();
			defaultInspector.onGUIHandler = () => { DrawDefaultInspector(); };
			root.Add(defaultInspector);

			return root;
		}
	}
}