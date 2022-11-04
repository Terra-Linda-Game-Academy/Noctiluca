using System;
using UnityEngine;

namespace Levels {
    public abstract class TileEntityController<E, C> : MonoBehaviour 
    where E : TileEntity<E, C> 
    where C : TileEntityController<E, C> {
        private E entity;
        private Guid roomId;
        private bool initted;

        protected E Entity => entity;
        protected Guid RoomId => roomId;

        public void Init(E entity, Guid roomId) {
            if (!initted) {
                this.entity = entity; 
                this.roomId = roomId;
                initted = true;
            } else {
                Debug.LogWarning("Init called more than once on a TileEntityController!");
            }
        }

        private void OnDestroy() => entity.RemoveInstance(roomId);
    }
}