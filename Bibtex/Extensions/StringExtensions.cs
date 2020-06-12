namespace Bibtex.Extensions
{
    /// <summary>
    /// Contains useful extension methods for strings commonly found in Bibtex.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Trims spaces, quotes, braces, tabs and new lines from both ends of the source string.
        /// </summary>
        /// <param name="str">Source string</param>
        /// <returns></returns>
        public static string TrimIgnoredCharacters(this string str) => str.Trim(' ', '"', '{', '}', '\t', '\r', '\n');

        /// <summary>
        /// Removes all braces in the source string.
        /// </summary>
        /// <param name="str">Source string</param>
        /// <returns></returns>
        public static string RemoveBraces(this string str) => str.Replace("{", "").Replace("}", "");
    }
}