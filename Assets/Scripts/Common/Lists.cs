using System;
using System.Collections.Generic;
using System.Linq;
namespace GGJ.Common {
    public static class Lists {
        public static IEnumerable<List<T>> Permute<T>(this IEnumerable<T> sequence) {
            if (sequence == null) {
                yield break;
            }

            var list = sequence.ToList();

            if (!list.Any()) {
                yield return Enumerable.Empty<T>().ToList();
            } else {
                var startingElementIndex = 0;

                foreach (var startingElement in list) {
                    var index = startingElementIndex;
                    var remainingItems = list.Where((e, i) => i != index);

                    foreach (var permutationOfRemainder in remainingItems.Permute()) {
                        yield return startingElement.Concat(permutationOfRemainder).ToList();
                    }

                    startingElementIndex++;
                }
            }
        }

        private static IEnumerable<T> Concat<T>(this T firstElement, IEnumerable<T> secondSequence) {
            yield return firstElement;
            if (secondSequence == null) {
                yield break;
            }

            foreach (var item in secondSequence) {
                yield return item;
            }
        }

        public static List<T> Of<T>(T type) {
            return new List<T> {type};
        }

        // Binary Search by delegation to int because C# doesn't have a fucking BinarySearch by Comparison
        public static T BinarySearchBy<T, K>(
            this List<T> edges,
            K key,
            Func<T, K> lambda
        ) where K : IComparable<K> {
            var min = 0;
            var max = edges.Count - 1;
            while (min <= max) {
                var mid = (min + max) / 2;
                var element = edges[mid];
                var elementKey = lambda(element);
                var comparison = elementKey.CompareTo(key);
                if (comparison == 0) {
                    return element;
                }

                if (comparison < 0) {
                    max = mid - 1;
                } else {
                    min = mid + 1;
                }
            }

            return default;
        }
    }
}