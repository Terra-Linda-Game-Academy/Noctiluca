using System;
using Levels;

namespace Util.ConcreteWeightedItems {
	[Serializable]
	public class WeightedRoom : WeightedItem<Room> {
		protected override Room GetItem() => item.Clone();
	}
}