namespace Bibtex.Extensions
{
    public static class StringExtensions
    {
        public static string TrimIgnoredCharacters(this string str) => str.Trim(' ', '"', '{', '}', '\t', '\r', '\n');

        public static string ReplaceBraces(this string str) => str.Replace("{", "").Replace("}", "");
    }
}