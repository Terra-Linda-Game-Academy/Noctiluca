using System;
using UnityEngine;

namespace Levels {
    [Serializable]
    public abstract class SimpleTile : ITile {
        public abstract string Name { get; }
        public abstract Vector2Int Position { get; }
        public abstract void Init(GameObject obj, Room room);
        public void Init(GameObject obj, Guid roomId, Room room) => Init(obj, room);
    }
}