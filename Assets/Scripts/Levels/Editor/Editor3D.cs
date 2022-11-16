using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Levels.Editor {
    public abstract class Editor3D {
        private SerializedObject target;
        
        public abstract IEnumerator<Property3D> Props { get; } //todo: IEnumerator of SelectionRegion or whatever

        public Editor3D(Object target) {
            this.target = new SerializedObject(target);
        }

        public abstract void DrawPreview();
    }
}