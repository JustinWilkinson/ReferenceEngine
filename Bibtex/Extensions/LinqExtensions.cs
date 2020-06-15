using System.Collections.Generic;
using System.Linq;

namespace Bibtex.Extensions
{
    /// <summary>
    /// Class containing extensions for Linq.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Checks if an IEnumerable has content.
        /// </summary>
        /// <typeparam name="T">Type of the enumerable</typeparam>
        /// <param name="enumerable">Enumerable to check for content.</param>
        /// <returns>True if the enumerable is not null and has at least one element, otherwise false.</returns>
        public static bool HasContent<T>(this IEnumerable<T> enumerable) => enumerable != null && enumerable.Any();
    }
}