using System.Collections.Generic;
using UnityEngine;

namespace Levels { 
    [CreateAssetMenu(fileName = "Room", menuName = "Room", order = 0)]
    public class Room : ScriptableObject {
        [SerializeField] private Vector3Int size;
        public Vector3Int Size => size;

        /// <Summary> values: in [-128, -64) are pits, in [-64, 0) are walls, [0, 127] are valid heights </Summary>
        [SerializeField] private sbyte[] heightMap;
        
        [SerializeField] private List<BaseTileEntity> tileEntities;
        [SerializeReference] private List<ITile> tiles;

        public IEnumerable<ITile> Tiles {
            get {
                foreach (var te in tileEntities) yield return te;
                foreach (var t in tiles) yield return t;
            }
        }
    }
}