using System.Collections.Generic;
using UnityEngine;

namespace Levels { 
    [CreateAssetMenu(fileName = "Room", menuName = "Room", order = 0)]
    public class Room : ScriptableObject {
        [SerializeField] private Vector3Int size;
        public Vector3Int Size => size;

        /// <Summary> values: in [-128, -64) are pits, in [-64, 0) are walls, [0, 127] are valid heights </Summary>
        [SerializeField] private sbyte[] heightMap;
        
        [SerializeField] private List<TileAsset> tileAssets;
        [SerializeReference] private List<SimpleTile> tiles;


        public IEnumerable<TileAsset> TileAssets {
            get {
                foreach (var ta in tileAssets) yield return ta; 
            }
        }

        public IEnumerable<SimpleTile> Tiles {
            get {
                foreach (var t in tiles) yield return t;
            }
        }

        public IEnumerable<ITile> AllTiles {
            get {
                foreach (var ta in tileAssets) yield return ta; 
                foreach (var t in tiles) yield return t;
            }
        }
    }
}