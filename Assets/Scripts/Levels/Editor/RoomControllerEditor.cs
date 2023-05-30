using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Levels.Editor {
    [CustomEditor(typeof(RoomController))]
    public class RoomControllerEditor : UnityEditor.Editor { 
        public override VisualElement CreateInspectorGUI() {
            VisualElement element = new VisualElement();

            var roomProp = serializedObject.FindProperty("room");
            element.Add(new PropertyField(roomProp, "Room"));

            if (roomProp.objectReferenceValue is not null) {
                SerializedObject room = new SerializedObject(roomProp.objectReferenceValue);
                var sizeProp = room.FindPropertyOrFail("size");
                element.Add(new PropertyField(sizeProp));
            }

            element.Bind(serializedObject);
            return element;
        }
    }
}