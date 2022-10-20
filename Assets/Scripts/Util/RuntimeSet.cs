using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Util {
    public class RuntimeSet<T> : ScriptableObject {
        private HashSet<T> items = new HashSet<T>();

        public IEnumerator<T> Items => items.GetEnumerator();
        
        public void Add([NotNull] T t) {
            if (!items.Contains(t)) items.Add(t);
        }

        public void Remove([NotNull] T t) {
            items.Remove(t);
        }
    }
}