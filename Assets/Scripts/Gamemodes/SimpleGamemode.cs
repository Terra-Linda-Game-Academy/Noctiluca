using System.Collections;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamemodes {
    public class SimpleGamemode : Gamemode {
        [SerializeField] private SceneReference scene;

        protected override IEnumerator _OnStart() {
            yield return SceneManager.LoadSceneAsync(scene);
        }

        protected override IEnumerator _OnEnd() { yield break; }
    }
}