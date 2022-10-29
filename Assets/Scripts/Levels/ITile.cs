using System;
using UnityEngine;
using Util;

namespace Levels {
    public interface ITile {
        public string Name { get; }
        public Vector3 Pos { get; }
        public TileDirection Rot { get; }
        public Option<Vector3> Size { get; }

        public void TileInit(ref GameObject component, Guid roomId);
    }
}