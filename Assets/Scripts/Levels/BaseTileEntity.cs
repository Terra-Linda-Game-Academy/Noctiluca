using System;
using UnityEngine;

namespace Levels {
    public abstract class BaseTileEntity : ScriptableObject {
        public abstract string Name { get; }
        
        public abstract Vector3Int Size { get; }
        

        [SerializeField] private Vector3Int pos;
        public Vector3Int Pos => pos;

        public abstract void Init(GameObject obj, Guid roomId);
    }
}