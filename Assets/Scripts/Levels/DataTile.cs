using System;
using UnityEngine;

namespace Levels {
    public abstract class DataTile : TileAsset {
        public sealed override bool CreateGameObject => false;
        public sealed override Vector3Int Position => Vector3Int.zero;
        public sealed override void Init(GameObject obj, Guid roomId) { }
    }
}