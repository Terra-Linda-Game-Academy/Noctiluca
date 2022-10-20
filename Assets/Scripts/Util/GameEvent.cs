using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Util {
    [CreateAssetMenu(fileName = "Game Event", menuName = "Utilities/Game Event", order = 0)]
    public class GameEvent : ScriptableObject {
        private List<GameEventListener> listeners;

        public void Register([NotNull] GameEventListener l) {
            if (!listeners.Contains(l)) listeners.Add(l);
        }

        public void Unregister([NotNull] GameEventListener l) {
            listeners.Remove(l);
        }
        
        public void Raise() {
            for (int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].HandleEvent();
            }
        }
    }
}