using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceEngine.Bibtex.Extensions
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
        /// <returns>The trimmed string.</returns>
        public static string TrimIgnoredCharacters(this string str) => str.Trim(' ', '"', '{', '}', '\t', '\r', '\n');

        /// <summary>
        /// Removes all braces in the source string.
        /// </summary>
        /// <param name="str">Source string</param>
        /// <returns>The source string with any braces removed.</returns>
        public static string RemoveBraces(this string str) => str.Replace("{", "").Replace("}", "");

        /// <summary>
        /// Removes the first character from a string if it matches any of the provided characters.
        /// </summary>
        /// <param name="str">Source string</param>
        /// <returns>The source string with the first character removed if it matches any of the provided characters.</returns>
        public static string RemoveFromStart(this string str, params char[] chars) => str.Length > 0 && chars.Any(c => c == str[0]) ? str.Remove(0, 1) : str;

        /// <summary>
        /// Removes the final character from a string if it matches any of the provided characters.
        /// </summary>
        /// <param name="str">Source string</param>
        /// <returns>The source string with the final character removed if it matches any of the provided characters.</returns>
        public static string RemoveFromEnd(this string str, params char[] chars)
        {
            var finalIndex = str.Length - 1;
            return chars.Any(c => c == str[finalIndex]) ? str.Remove(finalIndex) : str;
        }

        /// <summary>
        /// Split a string on a given character, provided it is not contained in quotes or braces.
        /// </summary>
        /// <param name="input">String to split.</param>
        /// <param name="splitChar">Character to split on.</param>
        /// <returns>An IEnumerable of strings</returns>
        public static IEnumerable<string> SplitOnUnquotedCharacter(this string input, char splitChar)
        {
            var leftBraceCount = 0;
            var rightBraceCount = 0;
            var inQuotes = false;

            StringBuilder current = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == '"')
                {
                    inQuotes = leftBraceCount == rightBraceCount ? !inQuotes : inQuotes;
                    current.Append(c);
                }
                else if (!inQuotes && leftBraceCount == rightBraceCount && c == splitChar)
                {
                    yield return current.ToString();
                    current.Clear();
                }
                else
                {
                    if (c == '{')
                    {
                        leftBraceCount++;
                    }
                    else if (c == '}')
                    {
                        rightBraceCount++;
                    }

                    current.Append(c);
                }
            }

            yield return current.ToString();
        }

        /// <summary>
        /// Performs string variable substitution/concatenation, by splitting a string on an unquoted character
        /// and replacing any instances of the substitution key in the resulting collection with the substitution value before rejoining the string with the specified string.
        /// </summary>
        /// <param name="toSubstitute">String to perform substitution on.</param>
        /// <param name="substitution">Key value pair for substitution.</param>
        /// <param name="splitChar">Character to split string on.</param>
        /// <param name="join">String used to re-join concatenated strings, defaults to the split character if null or not provided.</param>
        /// <param name="func">Function to run on the split strings.</param>
        /// <returns></returns>
        public static string Substitute(this string toSubstitute, KeyValuePair<string, string> substitution, char splitChar = ' ', string join = null, Func<string, string> func = null)
        {
            if (!toSubstitute.Contains(substitution.Key))
            {
                return toSubstitute;
            }

            join ??= splitChar.ToString();

            if (func != null)
            {
                return string.Join(join, toSubstitute.SplitOnUnquotedCharacter(splitChar).Select(str => substitution.Key == str.Trim() ? func(str.Replace(substitution.Key, substitution.Value)) : func(str)));
            }
            else
            {
                return string.Join(join, toSubstitute.SplitOnUnquotedCharacter(splitChar).Select(str => substitution.Key == str.Trim() ? str.Replace(substitution.Key, substitution.Value) : str));
            }
        }
    }
}