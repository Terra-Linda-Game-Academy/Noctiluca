using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Levels.Editor {
    [CustomEditor(typeof(RoomController))]
    public class RoomControllerEditor : UnityEditor.Editor { 
        public override VisualElement CreateInspectorGUI() {
            VisualElement element = new VisualElement();

            var roomProp = serializedObject.FindProperty("room");
            element.Add(new PropertyField(roomProp, "room"));
            
            var sizeProp = roomProp.FindPropertyRelative("size");
            element.Add(new PropertyField(sizeProp));
            
            
            if (roomProp.objectReferenceValue is not null) {
                
            }

            element.Bind(serializedObject);
            return element;
        }
    }
}