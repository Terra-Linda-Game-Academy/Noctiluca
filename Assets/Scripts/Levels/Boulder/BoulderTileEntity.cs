using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    [CreateAssetMenu(fileName = "New Boulder Tile Entity", menuName = "Tiles/Boulder Tile Entity")]
    public class BoulderTileEntity : TileEntity<BoulderTileEntity, BoulderController> {
        public float Mass = 1f;
        public float Radius = 0.5f;

        public override string Name { get; } = "BoulderTileEntity";
        public override Vector3Int Position { get; } = Vector3Int.zero;

        protected override void Init(BoulderController controller) {
            controller.gameObject.AddComponent<Rigidbody>().mass = Mass;
            controller.gameObject.AddComponent<SphereCollider>().radius = Radius;
        }
    }

    public class BoulderController : TileEntityController<BoulderTileEntity, BoulderController> {
    }
}

