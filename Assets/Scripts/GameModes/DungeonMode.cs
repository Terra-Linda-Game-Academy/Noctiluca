using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameModes {
    public class DungeonGameMode : GameMode {
        protected override IEnumerator _OnStart() {
            Debug.Log("going to dungeon");
            SceneManager.LoadScene("Game");
            yield break;
        }

        protected override IEnumerator _OnEnd() {
            Debug.Log("leaving dungeon");
            yield break;
        }
    }
}