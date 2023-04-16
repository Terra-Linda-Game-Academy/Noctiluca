using System;
using Levels;
using UnityEngine;
using Util.ConcreteWeightedItems;

namespace Util.ConcretePools {
	[Serializable]
	[CreateAssetMenu(menuName = "Levels/Room Pool", fileName = "Room Pool")]
	public class RoomPool : Pool<Room, WeightedRoom> { }
}