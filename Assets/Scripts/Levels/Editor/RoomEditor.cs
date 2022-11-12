using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Levels.Editor {
    [CustomEditor(typeof(Room))]
    public class RoomEditor : UnityEditor.Editor {

        public override VisualElement CreateInspectorGUI() {
            VisualElement element = new VisualElement();

            var dimensionsProp = serializedObject.FindProperty("dimensions");
            
            element.Add(new PropertyField(dimensionsProp, "dimensions"));

            element.Bind(serializedObject);
            return element;
        }
    }
}