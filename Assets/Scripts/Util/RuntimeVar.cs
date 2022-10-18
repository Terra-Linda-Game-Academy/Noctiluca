using System;
using UnityEngine;

namespace Util {
    public class RuntimeVar<T> : ScriptableObject {
        [NonSerialized] public T Value;
    }
}


