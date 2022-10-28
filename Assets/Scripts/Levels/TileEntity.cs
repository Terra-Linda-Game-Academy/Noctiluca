using UnityEngine;

namespace Levels {
    public abstract class TileEntity<T> : ScriptableObject {
        protected Vector3Int pos;
        protected TileDirection rot;
        protected Vector3Int size; // not dimension, for example the length of the x side is (size.x + 1) and something with size 0 is 1x1x1

        public Vector3Int Pos => pos;
        public TileDirection Rot => rot;
        public Vector3Int Size => size;
        
    }
}