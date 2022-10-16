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
        }

        public static Option<T> None() => new Option<T>(){ enabled = false };
        public static Option<T> Some(T value) => new Option<T>(){ enabled = true, value = value };

        public Option<U> Map<U>(Func<T, U> fn) {
            if (enabled) return Option<U>.Some(fn.Invoke(value));
            return Option<U>.None();
        }

        public Option<U> FlatMap<U>(Func<T, Option<U>> fn) {
            if (!enabled) return Option<U>.None();
            return fn.Invoke(value);
        }

        public void MapInPlace(Func<T, T> fn) {
            if (enabled) value = fn.Invoke(value);
        }
    }
}