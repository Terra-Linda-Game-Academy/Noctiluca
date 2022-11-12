using System.Collections.Generic;

namespace Levels.Editor {
    public abstract class Inspector3D {
        public abstract IEnumerator<int> selectionRegions { get; } //todo: IEnumerator of SelectionRegion or whatever
    }
}