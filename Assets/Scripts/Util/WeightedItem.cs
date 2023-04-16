using System;
using UnityEngine;

namespace Util {
	[Serializable]
	public class WeightedItem<T> {
		[SerializeField] protected T item;

		public T Item {
			get => GetItem();
			set => item = value;
		}

		[SerializeField] private int weight;

		public int Weight {
			get => weight > 0 ? weight : 1;
			set => weight = value;
		}

		public bool unique;

		[NonSerialized] public bool Used;

		public WeightedItem() { }

		public WeightedItem(T item) { this.item = item; }

		/// <summary>
		/// Allows for certain weighted items to return clones
		/// </summary>
		/// <returns>By default a reference to the item, in certain cases a clone of the item</returns>
		protected virtual T GetItem() => item;
	}
}