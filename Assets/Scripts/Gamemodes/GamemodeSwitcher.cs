using System.Collections;
using Main;
using UnityEngine;

//this is a utility class for use with no-code stuff like GameEventListeners
namespace Gamemodes {
    public class GamemodeSwitcher : MonoBehaviour {
        [SerializeField] private Gamemode target;

        public void SwitchMode() {
            IEnumerator action = App.GamemodeManager.SwitchMode(target);
            StartCoroutine(action);
        }
    }
}