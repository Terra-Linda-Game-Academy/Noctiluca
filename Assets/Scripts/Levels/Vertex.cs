using System.Runtime.InteropServices;
using UnityEngine;

namespace Levels {
    [StructLayout(LayoutKind.Sequential)]
    readonly struct Vertex {
        public const int Stride = (3 + 3 + 2) * sizeof(float); 
            
        public readonly Vector3 position;
        public readonly Vector3 normal;
        public readonly Vector2 uv;

        public Vertex(Vector3 position, Vector3 normal, Vector2 uv) {
            this.position = position;
            this.normal = normal;
            this.uv = uv;
        }
    }
}