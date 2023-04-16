using System;
using Levels;
using Util.ConcreteWeightedItems;

namespace Util.ConcretePools {
	[Serializable]
	public class ConnectionPool : Pool<Room.ConnectionPoint, WeightedConnection> { }
}