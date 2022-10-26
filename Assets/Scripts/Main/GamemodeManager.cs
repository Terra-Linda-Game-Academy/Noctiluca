using System.Collections;
using Gamemodes;
using UnityEngine;

namespace Main {
    public class GamemodeManager : MonoBehaviour {
        private bool isSwitchingMode;
        private Gamemode currentMode;
        
        public IEnumerator SwitchMode(Gamemode mode) {
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