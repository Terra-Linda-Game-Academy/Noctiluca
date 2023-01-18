using UnityEditor;
using UnityEngine;

namespace Levels.Editor {
    public abstract class Property3D {
        public abstract void DrawProperty();
    }

    public abstract class Property3D<T> : Property3D {
        private T current;
        public T Current => current;

        public Property3D(T initial) {
            current = initial;
        }
    }
}