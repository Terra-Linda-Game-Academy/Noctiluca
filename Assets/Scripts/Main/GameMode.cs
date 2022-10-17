using System.Collections;

namespace Main {
    public abstract class GameMode {
        public GameModeState State { get; protected set; } = GameModeState.Ended;

        protected abstract IEnumerator _OnStart();
        protected abstract IEnumerator _OnEnd();

        public IEnumerator OnStart() {
            if (State != GameModeState.Ended) yield break;
            State = GameModeState.Starting;
            yield return _OnStart();
            State = GameModeState.Started;
        }

        public IEnumerator OnEnd() {
            if (State != GameModeState.Started) yield break;
            State = GameModeState.Ending;
            yield return _OnEnd();
            State = GameModeState.Ended;
        }
    }
}