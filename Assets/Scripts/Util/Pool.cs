using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util {
	[Serializable]
	public class Pool<T, Tw> : ScriptableObject where Tw : WeightedItem<T>, new() {
		//real type is List<Tw>
		[SerializeReference] private List<object> _items = new();

		public int Count => _items.Count;

		public Option<T> One() => One(_ => true);

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

			return false;
		}
	}
}