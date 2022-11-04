using System;
using UnityEngine;

namespace Levels {
    public abstract class BaseTileEntity : ScriptableObject, ITile {
        public abstract string Name { get; }
        public abstract Vector3Int Position { get; }
        
        public abstract void Init(GameObject obj, Guid roomId);
    }
}