using System;
using UnityEngine;

namespace Util {
    [Serializable]
    public struct Option<T> {
        [SerializeField] private bool enabled;
        [SerializeField] private T value;

        public bool Enabled => enabled;
        public T Value {
            get {
                if (enabled) return value;
                throw new NoneReferenceException(typeof(T));
            }
            set {
                enabled = true;
                this.value = value;
            }
        }

        public static Option<T> None() => new Option<T>() { enabled = false };
        public static Option<T> Some(T value) => new Option<T>() { enabled = true, value = value };

        public void Empty() => enabled = false;
    }
}