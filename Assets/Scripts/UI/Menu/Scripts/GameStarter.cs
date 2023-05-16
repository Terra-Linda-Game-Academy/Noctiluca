using Main;
using UnityEngine;

namespace UI.Menu.Scripts {
	public class GameStarter : MonoBehaviour {
		public void StartGame() => App.GameModeManager.StartGame();
	}
}