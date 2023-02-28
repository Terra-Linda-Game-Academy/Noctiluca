using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Levels {
    public interface ITile {
        /// <summary>
        /// What to name the created GameObject
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Where to place the created GameObject
        /// </summary>
        public Vector3Int Position { get; }
        
        /// <summary>
        /// Initialize the given GameObject
        /// </summary>
        /// <param name="obj">The GameObject to initialize</param>
        /// <param name="roomId">
        /// The ID of the current room, to support having multiple of the same room loaded at once
        /// </param>
        public void Init(GameObject obj, Guid roomId);
    }
} 