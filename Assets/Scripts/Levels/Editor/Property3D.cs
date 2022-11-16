using UnityEditor;
using UnityEngine;

namespace Levels.Editor {
    public class Property3D {
        private Bounds selectionRegion;
        public Bounds SelectionRegion => selectionRegion;

        private SerializedProperty target;
    }
}