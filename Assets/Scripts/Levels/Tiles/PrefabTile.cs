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
        
        [SerializeField, HideInInspector] 
        private BoundsInt bounds;
        
        public override string Name => name.OrElse(prefab.name);
        public override Vector3Int Position => bounds.min;

        public override void Init(GameObject obj) {
            Object.Instantiate(prefab, obj.transform);
        }
    }
}