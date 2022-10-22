using System.Collections;
using Levels;
using UnityEngine;

namespace GameModes {
    public class DungeonGameMode : GameMode {
        [SerializeField] private LevelStack levels;
        
        protected override IEnumerator _OnStart() { throw new System.NotImplementedException(); }
        protected override IEnumerator _OnEnd() { throw new System.NotImplementedException(); }

        protected override IEnumerator _OnStarted() { throw new System.NotImplementedException(); }
    }
}