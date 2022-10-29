using UnityEngine;

namespace Levels {
    public class TileEntityHelper : MonoBehaviour {
        private BaseTileEntity baseObj;

        public BaseTileEntity BaseObj {
            set => baseObj ??= value;
        }
    }
}