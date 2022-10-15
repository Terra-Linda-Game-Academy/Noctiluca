using UnityEngine;
using UnityEngine.Events;

namespace Util {
    public class GameEventListener : MonoBehaviour {
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] private UnityEvent response;

        public void OnEnable() => gameEvent.Register(this);
        public void OnDisable() => gameEvent.Unregister(this);

        public void HandleEvent() => response?.Invoke();
    }
}