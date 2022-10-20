using System.Collections.Generic;
using UnityEngine;

namespace Levels {
    [CreateAssetMenu(fileName = "Level Stack", menuName = "Levels/Level Stack", order = 0)]
    public class LevelStack : ScriptableObject {
        [SerializeField] private List<Level> levels;
    }
}