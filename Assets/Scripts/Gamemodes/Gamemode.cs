using System.Collections;
using UnityEngine;

namespace GameModes {
    public abstract class GameMode : ScriptableObject {
        public GameModeState State { get; protected set; } = GameModeState.Ended;

        protected abstract IEnumerator _OnStart();
        protected virtual IEnumerator _OnStarted() { yield break; }
        protected abstract IEnumerator _OnEnd();
        

        public IEnumerator OnStart() {
            if (State != GameModeState.Ended) yield break;
            State = GameModeState.Starting;
            yield return _OnStart();
            State = GameModeState.Started;
        }

        public IEnumerator OnStarted() {
            if (State != GameModeState.Started) yield break;
            yield return _OnStarted();
        }

        public IEnumerator OnEnd() {
            if (State != GameModeState.Started) yield break;
            State = GameModeState.Ending;
            yield return _OnEnd();
            State = GameModeState.Ended;
        }
    }
}