using System.Collections.Generic;
using UnityEngine;

namespace Levels { 
    [CreateAssetMenu(fileName = "Room", menuName = "Room", order = 0)]
    public class Room : ScriptableObject {
        [SerializeField] private Vector3Int dimensions;
        [SerializeField] private byte[] heightMap; //values < 0 represent nonexistent terrain (inside a wall)
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