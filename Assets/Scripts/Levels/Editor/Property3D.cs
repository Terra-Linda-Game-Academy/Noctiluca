using UnityEditor;
using UnityEngine;

namespace Levels.Editor {
    public abstract class Property3D {
        public abstract Bounds SelectionRegion { get; }

    }

    public abstract class Property3D<T> : Property3D {
        
        public Property3D(SerializedProperty property) {
            serializedProperty = property;
        }
    }
}