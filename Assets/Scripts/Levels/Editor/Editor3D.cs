using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Levels.Editor {
    public abstract class Editor3D {
        public abstract IEnumerable<Bounds> SelectionRegions { get; }

        public abstract void DrawEditor();
    }

    public abstract class AssetEditor3D<T> : Editor3D where T : TileAsset {
        private readonly T target;

        public AssetEditor3D(T target) {
            this.target = target;
        }

        public sealed override void DrawEditor() {
            var so = new SerializedObject(target);
            DrawEditor(so);
            so.ApplyModifiedProperties();
        }

        protected abstract void DrawEditor(SerializedObject so);
    }
}