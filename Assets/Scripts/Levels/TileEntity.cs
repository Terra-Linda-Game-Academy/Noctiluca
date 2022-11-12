using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public abstract class TileEntity<E, C> : BaseTileEntity 
    where E : TileEntity<E, C> // C for "Controller"
    where C : TileEntityController<E, C> { // E for "Entity" (this object)
        private Dictionary<Guid, C> instances;
        protected C GetInstance(Guid roomId) => instances[roomId];
        
        protected abstract void Init(C component);

        public override void Init(GameObject obj, Guid roomId) {
            C controller = obj.AddComponent<C>();
            instances.Add(roomId, controller);
            controller.Init((E) this, roomId);
            Init(controller);
        }

        public void RemoveInstance(Guid roomId) => instances.Remove(roomId);
    }
}