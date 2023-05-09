using System.Collections;
using Levels;
using UnityEngine;

namespace GameModes {
    public class DungeonGameMode : GameMode {
        [SerializeField] private LevelStack levels;

        protected override IEnumerator _OnStart() {
            Debug.Log("going to dungeon");
            yield break;
        }
        protected override IEnumerator _OnEnd() { throw new System.NotImplementedException(); }
    }
}