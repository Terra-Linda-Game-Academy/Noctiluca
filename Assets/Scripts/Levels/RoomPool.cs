using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Levels {
    [CreateAssetMenu(fileName = "Room Pool", menuName = "Levels/Room Pool", order = 0)]
    public class RoomPool : ScriptableObject, ISerializationCallbackReceiver, IEnumerable<Room> {
        [SerializeField] private List<WeightedItem<Room>> rooms;
        [NonSerialized] private RandomPool<Room> randomRooms;
        
        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() {
            randomRooms = new RandomPool<Room>(rooms);
        }
        
        //todo: add restrictions for the 

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator<Room> GetEnumerator() => randomRooms.GetEnumerator();
    }
}