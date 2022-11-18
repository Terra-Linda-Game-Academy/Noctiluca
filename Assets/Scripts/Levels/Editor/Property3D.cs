using UnityEditor;
using UnityEngine;

namespace Levels.Editor {
    public abstract class Property3D {
        public abstract Bounds SelectionRegion { get; }

        protected readonly SerializedProperty serializedProperty;
        
        public Property3D(SerializedProperty property) {
            serializedProperty = property;
        }
    }
}