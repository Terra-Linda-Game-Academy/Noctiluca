using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    public abstract class TileEntity<E, C> : TileAsset 
    where E : TileEntity<E, C> // E for "Entity" (this object)
    where C : TileEntityController<E, C> { // C for "Controller"
        public override bool CreateGameObject => true;
        
        private Dictionary<Guid, C> instances;
        protected C GetInstance(Guid roomId) => instances[roomId];
        
        protected abstract void Init(C component, Room room);

        public sealed override void Init(GameObject obj, Guid roomId, Room room) {
            var controller = obj.AddComponent<C>();
            instances.Add(roomId, controller);
            controller.Init((E) this, roomId);
            Init(controller, room);
        }

        public void RemoveInstance(Guid roomId) => instances.Remove(roomId);
    }
}