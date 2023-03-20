using System;
using UnityEngine;
using Util;
using Object = UnityEngine.Object;

namespace Levels.Tiles {
    [Serializable]
    public class PrefabTile : SimpleTile {
        [SerializeField, Tooltip("The prefab to place in the room")] 
        private GameObject prefab;
        
        [SerializeField, Tooltip("By default, will use the name of the prefab")]
        private Option<string> name;

        [SerializeField, Tooltip("Where the tile should be located in the room")] 
        private Vector2Int position;

        public override string Name => name.OrElse(prefab.name);
        public override Vector2Int Position => position;

        public override void Init(GameObject obj, Room room) {
            Object.Instantiate(prefab, obj.transform);
        }
    }
}