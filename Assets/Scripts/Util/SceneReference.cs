using System;
using UnityEngine;

namespace Util {
    [Serializable]
    public class SceneReference {
        [SerializeField] private string scenePath;
        public string ScenePath => scenePath;
    }
}