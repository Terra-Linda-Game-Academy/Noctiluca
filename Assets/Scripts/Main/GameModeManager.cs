using GameModes;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class GameModeManager : MonoBehaviour
    {
        private bool isSwitchingMode;
        private GameMode currentMode;
        public IEnumerator SwitchMode(GameMode mode)
        {
            yield return new WaitUntil(() => !isSwitchingMode);
            if (currentMode == mode) yield break;

            isSwitchingMode = true;
            //todo: enable loading screen;

            if (currentMode != null) yield return currentMode.OnEnd();
            currentMode = mode;
            yield return currentMode.OnStart();

            //todo: disable loading screen
            isSwitchingMode = false;
        }
    }
}