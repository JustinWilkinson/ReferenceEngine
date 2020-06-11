using Bibtex.Extensions;
using NUnit.Framework;

namespace Test.Bibtex.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [TestCase("{ Some value here }", ExpectedResult = "Some value here")]
        [TestCase("{ Some value here \t \r \n}", ExpectedResult = "Some value here")]
        [TestCase(@"{ "" Some value here ""}", ExpectedResult = "Some value here")]
        public string TrimIgnoredCharacters_RemovesIgnoredBibtexCharacters_Successful(string toTrim)
        {
            return toTrim.TrimIgnoredCharacters();
        }

        [TestCase("{Some value here}", ExpectedResult = "Some value here")]
        [TestCase("{Some value with {nested value here}}", ExpectedResult = "Some value with nested value here")]
        public string ReplaceBraces_RemovesBraces_Successfully(string toTrim)
        {
            return toTrim.ReplaceBraces();
        }
    }
}