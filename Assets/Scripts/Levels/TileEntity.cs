using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public abstract class TileEntity<T> : BaseTileEntity where T : MonoBehaviour {
        public abstract string Name { get; }
        
        public abstract Vector3Int Size { get; }
        

        [SerializeField] private Vector3Int pos;
        public Vector3Int Pos => pos;
        

        private Dictionary<Guid, T> instances;

        protected T GetInstance(Guid roomId) => instances[roomId];

        public void TileInit(GameObject obj, Guid roomId) {
            T component = obj.AddComponent<T>();
            instances[roomId] = component;
            TileEntityInit(component);
            TileEntityHelper helper = obj.GetComponent<TileEntityHelper>();
            if (helper is not null) {
                helper.BaseObj = this;
            }
        }

        public abstract void TileEntityInit(T component);
    }
}