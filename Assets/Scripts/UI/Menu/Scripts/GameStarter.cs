using GameModes;
using Main;
using UnityEngine;

namespace UI.Menu.Scripts {
	public class GameStarter : MonoBehaviour {
		public void StartGame() {
			StartCoroutine(App.GameModeManager.SwitchMode(new DungeonGameMode()));
		}
	}
}