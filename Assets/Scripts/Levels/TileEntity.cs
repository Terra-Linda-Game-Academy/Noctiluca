using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public abstract class TileEntity<C, E> : BaseTileEntity 
    where C : TileEntityController<C, E> // C for "Controller"
    where E : TileEntity<C, E> { // E for "tileEntity" (this object)
        private Dictionary<Guid, C> instances;
        protected C this[Guid roomId] => instances[roomId];
        
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