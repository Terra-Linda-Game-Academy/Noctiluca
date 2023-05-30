using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Util {
	[Serializable]
	public class RandomPool<T> : IEnumerable<T>, IEquatable<RandomPool<T>> {
		private readonly HashSet<WeightedItem<T>> items;

		public int Count => items.Count;

		public RandomPool(params T[] items) : this(items.AsEnumerable()) { }

		public RandomPool(IEnumerable<T> items) : this(items.Select(i => new WeightedItem<T>(i))) { }

		public RandomPool(Func<T, int> weightFn, params T[] items) : this(weightFn, items.AsEnumerable()) { }

		public RandomPool(Func<T, int> weightFn, IEnumerable<T> items) :
			this(items.Select(i => new WeightedItem<T>(i, weightFn.Invoke(i)))) { }

		public RandomPool(Func<T, (int, bool)> weightFn, params T[] items) : this(weightFn, items.AsEnumerable()) { }

		public RandomPool(Func<T, (int, bool)> weightFn, IEnumerable<T> items) :
			this(items.Select(i => {
				                  var (weight, unique) = weightFn.Invoke(i);
				                  return new WeightedItem<T>(i, weight, unique);
			                  })) { }

		public RandomPool(params (T, int)[] items) : this(items.AsEnumerable()) { }

		public RandomPool(IEnumerable<(T, int)> items) :
			this(items.Select(i => new WeightedItem<T>(i.Item1, i.Item2))) { }

		public RandomPool(params (T, int, bool)[] items) : this(items.AsEnumerable()) { }

		public RandomPool(IEnumerable<(T, int, bool)> items) :
			this(items.Select(i => new WeightedItem<T>(i.Item1, i.Item2, i.Item3))) { }

		public RandomPool(params WeightedItem<T>[] items) : this(items.AsEnumerable()) { }

		public RandomPool(IEnumerable<WeightedItem<T>> items) {
			this.items = new HashSet<WeightedItem<T>>();
			foreach (var weightedItem in items) { this.items.Add(weightedItem); }
		}

		public bool Equals(RandomPool<T> other) =>
			other is not null && items.Equals(other.items);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IEnumerator<T> GetEnumerator() {
			int totalWeight = 1;

			var availableItems = new HashSet<WeightedItem<T>>(items);

			while (availableItems.Count > 0) {
				totalWeight = availableItems.Sum(i => i.Weight);

				int             rand          = Random.Range(0, totalWeight + 1);
				int             currentWeight = 0;
				WeightedItem<T> toReturn      = null;
				foreach (WeightedItem<T> i in availableItems) {
					currentWeight += i.Weight;
					if (currentWeight >= rand) {
						toReturn = i;
						break;
					}
				}
				//todo: unique checking
				if (toReturn.Unique) {
					availableItems.Remove(toReturn);
					totalWeight -= toReturn.Weight;
				}

				yield return toReturn.Item;
			}

			//if no available items remaining, default to searching whole set
			while (true) { //todo: implement unique room exclusion
				int rand          = Random.Range(0, totalWeight);
				int currentWeight = 0;
				foreach (WeightedItem<T> i in items) {
					currentWeight += i.Weight;
					if (currentWeight >= rand) {
						yield return i.Item;
					}
				}
			}
		}

		public IEnumerable<WeightedItem<T>> NonRandom => items;


		/*public Option<T> One() => One(_ => true);

		public Option<T> One(Func<T, bool> predicate) {
			//turn list into correct type
			Tw[] typedItems                                      = new Tw[_items.Count];
			for (int i = 0; i < _items.Count; i++) typedItems[i] = (Tw) _items[i];

			//find all valid & nonunique/unused items
			Tw[] validItems = typedItems.Where(i => predicate(i.Item) && (!i.unique || !i.Used)).ToArray();

			//if none, try all valid items w/o unique check
			if (validItems.Length == 0) { validItems = typedItems.Where(i => predicate(i.Item)).ToArray(); }

			//if none, return nothing
			if (validItems.Length == 0) return Option<T>.None();

			int totalWeight = validItems.Sum(i => i.Weight);
			int rand        = Random.Range(0, totalWeight);

			int currentWeight = 0;
			foreach (Tw i in validItems) {
				currentWeight += i.Weight;
				if (rand < currentWeight) {
					if (i.unique) i.Used = true;
					return Option<T>.Some(i.Item);
				}
			}

			return Option<T>.None();
		}

		public IEnumerable<T> All() {
			T[] typedItems                                       = new T[_items.Count];
			for (int i = 0; i < _items.Count; i++) typedItems[i] = ((Tw) _items[i]).Item;
			return typedItems;
		}

		public void Fill(IEnumerable<T> items) {
			_items.Clear();
			foreach (T item in items) { _items.Add(new Tw {Item = item}); }
		}

		public bool HasAny(Func<T, bool> predicate) {
			foreach (Tw i in _items) {
				if (predicate(i.Item)) return true;
			}
		}*/
	}
}