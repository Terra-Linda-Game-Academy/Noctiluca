using System.Collections;
using UnityEngine;

namespace Gamemodes {
    public abstract class Gamemode : ScriptableObject {
        public GamemodeState State { get; protected set; } = GamemodeState.Ended;

        protected abstract IEnumerator _OnStart();
        protected abstract IEnumerator _OnEnd();

        public IEnumerator OnStart() {
            if (State != GamemodeState.Ended) yield break;
            State = GamemodeState.Starting;
            yield return _OnStart();
            State = GamemodeState.Started;
        }

        public IEnumerator OnEnd() {
            if (State != GamemodeState.Started) yield break;
            State = GamemodeState.Ending;
            yield return _OnEnd();
            State = GamemodeState.Ended;
        }
    }
}