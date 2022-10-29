using System;
using UnityEngine;

namespace Levels {
    public abstract class TileEntityController<C, E> : MonoBehaviour 
    where C : TileEntityController<C, E> 
    where E : TileEntity<C, E> {
        private E entity;
        private Guid roomId;
        private bool initted;

        protected E Entity => entity;
        protected Guid RoomId => roomId;

        public Vector3Int Pos => entity.Pos;

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