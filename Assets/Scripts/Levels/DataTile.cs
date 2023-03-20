using System;
using UnityEngine;

namespace Levels {
    public abstract class DataTile : TileAsset {
        public sealed override bool CreateGameObject => false;
        public sealed override Vector2Int Position => Vector2Int.zero;
        public sealed override void Init(GameObject obj, Guid roomId, Room room) { }
    }
}