using System;
using System.Collections.Generic;
using System.Linq;

namespace Util {
	public static class EnumerableExtensions {
		public static Option<T> One<T>(this IEnumerable<T> collection) {
			using var enumerator = collection.GetEnumerator();
			return !enumerator.MoveNext() ? Option<T>.None() : Option<T>.Some(enumerator.Current);
		}

		public static T RandomOne<T>(this IEnumerable<T> enumerable) {
			if (enumerable == null) { throw new ArgumentNullException(nameof(enumerable)); }

			// note: creating a Random instance each call may not be correct for you,
			// consider a thread-safe static instance
			var r    = new Random();
			var list = enumerable as IList<T> ?? enumerable.ToList();
			return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
		}

		/// <summary>
		/// Uhhh i don't think this acutal works rn, use RandomOne() instead
		/// </summary>
		/// <param name="collection"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IEnumerable<T> Random<T>(this IEnumerable<T> collection) =>
			new RandomPool<T>(_ => 1, collection);

		public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, Func<T, int> weightFn) =>
			new RandomPool<T>(weightFn, collection);

		public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, Func<T, (int, bool)> weightFn) =>
			new RandomPool<T>(weightFn, collection);
	}
}