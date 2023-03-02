using System;
using UnityEngine;

namespace Util
{
    [Serializable]
    public class ScriptableVar<T>
    {
        [SerializeField] private bool useConstant;
        [SerializeField] private RuntimeVar<T> variable;
        [SerializeField] private T constant;

        public T Value => useConstant ? constant : variable.Value;
    }
}

