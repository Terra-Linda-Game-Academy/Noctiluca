using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Levels.Editor {
    [CustomEditor(typeof(Room))]
    public class RoomEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI() {
            VisualElement element = new VisualElement();

            var sizeProp = serializedObject.FindProperty("size");
            element.Add(new PropertyField(sizeProp, "size"));

            var button = new Button(FindRoomController) { text = "Edit in Scene View" };
            element.Add(button);
            
            element.Bind(serializedObject);
            return element;
        }
        
        private void FindRoomController() {
            RoomController[] presentControllers = FindObjectsOfType<RoomController>();
            foreach (var controller in presentControllers) {
                if (controller.Room != target) continue;
                Selection.activeObject = controller;
                Selection.activeTransform = controller.transform;
                return;
            }

            GameObject newObj = new GameObject(target.name);
            RoomController roomController = newObj.AddComponent<RoomController>();

            SerializedObject so = new SerializedObject(roomController);
            so.FindProperty("room").objectReferenceValue = target;
            so.ApplyModifiedProperties();
            
            Selection.activeObject = roomController;
            Selection.activeTransform = roomController.transform;
        }
    }
}