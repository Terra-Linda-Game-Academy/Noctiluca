using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Levels.Editor {
    [CustomEditor(typeof(Room))]
    public class RoomEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI() {
            VisualElement element = new VisualElement();

            var sizeProp = serializedObject.FindProperty("size");
            element.Add(new PropertyField(sizeProp, "size"));

            element.Bind(serializedObject);
            return element;
        }

        public override bool RequiresConstantRepaint() => true;
    }
}