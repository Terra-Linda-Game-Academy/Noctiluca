using System.Runtime.InteropServices;
using UnityEngine;

namespace Levels {
    [StructLayout(LayoutKind.Sequential)]
    readonly struct Vertex {
        public const int Stride = (3 + 3 + 2) * sizeof(float); 
            
        private readonly Vector3 position;
        private readonly Vector3 normal;
        private readonly Vector2 uv;
    }
}