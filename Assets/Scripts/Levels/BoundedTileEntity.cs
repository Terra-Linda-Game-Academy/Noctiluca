using UnityEngine;

namespace Levels {
    public class BoundedTileEntity<E, C> : TileEntity<E, C>
    where E : BoundedTileEntity<E, C>
    where C : TileEntityController<E, C> {
        public override string Name { get; }

        public override Vector3Int Position => bounds.min;

        private BoundsInt bounds;
        public Vector3Int Min => bounds.min;
        public Vector3Int Max => bounds.max;
        public Vector3Int Size => bounds.size;
        
        protected override void Init(C component) {
            
        }
    }
}