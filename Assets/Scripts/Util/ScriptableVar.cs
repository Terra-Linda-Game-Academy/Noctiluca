using System;
using UnityEngine;

namespace Util {
    [Serializable]
    public class ScriptableVar<T> {
        [SerializeField] private bool useConstant;
        [SerializeField] private RuntimeVar<T> runtimeValue;
        [SerializeField] private T constantValue;

        public T Value => useConstant ? constantValue : runtimeValue.Value;
    }
}

