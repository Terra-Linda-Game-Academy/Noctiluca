using UnityEditor;
using UnityEngine;

namespace Levels {
    [CreateAssetMenu(fileName = "Level", menuName = "Levels/Level", order = 1)]
    public class Level : ScriptableObject {
        public SceneAsset scene;
    }
}