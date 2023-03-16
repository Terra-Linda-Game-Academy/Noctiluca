using System;
using UnityEngine;

namespace Levels {
    public abstract class TileAsset : ScriptableObject, ITile {
        public abstract string Name { get; }
        public abstract Vector2Int Position { get; }
        public abstract bool CreateGameObject { get; }
        public abstract void Init(GameObject obj, Guid roomId);
    }
}