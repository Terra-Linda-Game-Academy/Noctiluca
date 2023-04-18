using System;
using UnityEngine;

namespace Util {
	[Serializable]
	public class WeightedItem<T> {
		[SerializeField] private T item;
		public T Item => item;

		[SerializeField, Min(1)] private int weight;
		public int Weight => weight;
		
		[SerializeField] private bool unique;
		public bool Unique => unique;
		
		public WeightedItem(T item, int weight = 1, bool unique = false) {
			this.item = item;
			this.weight = Math.Max(weight, 1);
			this.unique = unique;
		}

		/*/// <summary>
		/// Allows for certain weighted items to return clones
		/// </summary>
		/// <returns>By default a reference to the item, in certain cases a clone of the item</returns>
		protected virtual T GetItem() => item;*/
	}
}