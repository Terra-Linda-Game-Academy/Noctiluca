using System;
using System.Collections.Generic;

namespace Util {
    public static class EnumerableExtensions {
        public static Option<T> One<T>(this IEnumerable<T> collection) {
            using var enumerator = collection.GetEnumerator();
            return !enumerator.MoveNext() ? Option<T>.None() : Option<T>.Some(enumerator.Current);
        }
        
        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection) =>
            new RandomPool<T>(_ => 1, collection);

        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, Func<T, int> weightFn) =>
            new RandomPool<T>(weightFn, collection);
        
        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, Func<T, (int, bool)> weightFn) =>
            new RandomPool<T>(weightFn, collection);
    }
}