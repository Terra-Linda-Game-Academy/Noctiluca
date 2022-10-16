using System;
using UnityEngine;

namespace Util {
    [Icon("Editor/RuntimeVar Icon")]
    public class RuntimeVar<T> : ScriptableObject {
        [NonSerialized] public T Value;
    }
}


