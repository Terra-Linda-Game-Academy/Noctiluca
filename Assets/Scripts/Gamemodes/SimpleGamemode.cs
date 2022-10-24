using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamemodes {
    public class SimpleGamemode : Gamemode {
        [SerializeField] private SceneAsset scene;

        protected override IEnumerator _OnStart() { throw new System.NotImplementedException(); }

        protected override IEnumerator _OnEnd() { throw new System.NotImplementedException(); }
    }
}