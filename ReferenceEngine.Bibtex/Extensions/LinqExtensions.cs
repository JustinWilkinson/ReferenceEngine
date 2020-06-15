using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceEngine.Bibtex.Extensions
{
    /// <summary>
    /// Class containing extensions for Linq.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Checks if an IEnumerable has content.
        /// </summary>
        /// <typeparam name="T">Type of elements in the enumerable.</typeparam>
        /// <param name="enumerable">Enumerable to check for content.</param>
        /// <returns>True if the enumerable is not null and has at least one element, otherwise false.</returns>
        public static bool HasContent<T>(this IEnumerable<T> enumerable) => enumerable != null && enumerable.Any();


        /// <summary>
        /// Returns a boolean value indicating if a single element was found. This method will throw an exception if there is more than one matching element.
        /// </summary>
        /// <typeparam name="T">Type of elements in the source enumerable.</typeparam>
        /// <param name="source">Source enumerable to find match in.</param>
        /// <param name="value">Value of the single match, or the default for type T.</param>
        /// <returns>True if a single element was found, false if not.</returns>
        public static bool TryGetSingle<T>(this IEnumerable<T> source, out T value)
        {
            value = source.SingleOrDefault();
            return !EqualityComparer<T>.Default.Equals(value, default);
        }

        /// <summary>
        /// Returns a boolean value indicating if a single element was found matching the given predicate. This method will throw an exception if there is more than one matching element.
        /// </summary>
        /// <typeparam name="T">Type of elements in the source enumerable.</typeparam>
        /// <param name="source">Source enumerable to find match in.</param>
        /// <param name="predicate">Predicate used to find single match.</param>
        /// <param name="value">Value of the single match, or the default for type T.</param>
        /// <returns>True if a single element was found, false if not.</returns>
        public static bool TryGetSingle<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T value)
        {
            value = source.SingleOrDefault(predicate);
            return !EqualityComparer<T>.Default.Equals(value, default);
        }


        /// <summary>
        /// Returns a boolean value indicating if a single element was found. This method will throw an exception if there is more than one matching element.
        /// </summary>
        /// <typeparam name="T">Type of elements in the source enumerable.</typeparam>
        /// <param name="source">Source enumerable to find match in.</param>
        /// <param name="value">Value of the single match, or the default for type T.</param>
        /// <returns>True if a single element was found, false if not.</returns>
        public static bool TryGetFirst<T>(this IEnumerable<T> source, out T value)
        {
            value = source.FirstOrDefault();
            return !EqualityComparer<T>.Default.Equals(value, default);
        }

        /// <summary>
        /// Returns a boolean value indicating if a single element was found matching the given predicate. This method will throw an exception if there is more than one matching element.
        /// </summary>
        /// <typeparam name="T">Type of elements in the source enumerable.</typeparam>
        /// <param name="source">Source enumerable to find match in.</param>
        /// <param name="predicate">Predicate used to find single match.</param>
        /// <param name="value">Value of the single match, or the default for type T.</param>
        /// <returns>True if a single element was found, false if not.</returns>
        public static bool TryGetFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T value)
        {
            value = source.FirstOrDefault(predicate);
            return !EqualityComparer<T>.Default.Equals(value, default);
        }
    }
}