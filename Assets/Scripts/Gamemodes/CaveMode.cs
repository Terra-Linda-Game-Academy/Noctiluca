using System.Collections;
using Levels;
using UnityEngine;

namespace Gamemodes {
    public class CaveMode : Gamemode {
        [SerializeField] private LevelStack levels;
        
        protected override IEnumerator _OnStart() { throw new System.NotImplementedException(); }
        
        protected override IEnumerator _OnEnd() { throw new System.NotImplementedException(); }

    }
}