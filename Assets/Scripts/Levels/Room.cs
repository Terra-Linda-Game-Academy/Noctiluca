using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Levels { 
    [CreateAssetMenu(fileName = "Room", menuName = "Room", order = 0)]
    public class Room : ScriptableObject {
        [Serializable, Flags]
        public enum TileFlags : ushort {
            Wall = 0b0000000000000001,
            Pit  = 0b0000000000000010
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public readonly struct Tile {
            public const int Stride = sizeof(TileFlags) + sizeof(ushort);
            
            public readonly TileFlags flags;
            public readonly ushort packedHeight;

            public float Height => Mathf.HalfToFloat(packedHeight);

            public Tile(TileFlags flags, float height) {
                this.flags = flags;
                packedHeight = Mathf.FloatToHalf(height);
            }
        }

        [SerializeField] private Vector3Int size;
        public Vector3Int Size => size;

        /// <Summary> lower half </Summary>
        [SerializeField] public Tile[] tileMap;
        
        [SerializeField] private TileAsset[] tileAssets;
        [SerializeReference] private SimpleTile[] tiles;

        public Tile GetTileAt(int x, int z) {
            if (x < 0 || x >= size.x || z < 0 || z >= size.z) {
                return new Tile(TileFlags.Wall, 0);
            }
            int linearIndex = z * size.x + x;
            return tileMap[linearIndex];
        }

        public IEnumerable<TileAsset> TileAssets {
            get { foreach (var ta in tileAssets) yield return ta; }
        }

        public IEnumerable<SimpleTile> Tiles {
            get { foreach (var t in tiles) yield return t; }
        }

        public IEnumerable<ITile> AllTiles {
            get {
                foreach (var ta in tileAssets) yield return ta; 
                foreach (var t in tiles) yield return t;
            }
        }
    }
}