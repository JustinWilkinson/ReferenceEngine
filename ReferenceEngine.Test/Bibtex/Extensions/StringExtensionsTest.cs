using NUnit.Framework;
using ReferenceEngine.Bibtex.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceEngine.Test.Bibtex.Extensions
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
        public string ReplaceBraces_RemovesBraces_Successfully(string input)
        {
            return input.RemoveBraces();
        }

        [Test]
        public void SplitOnUnquotedCharacter_WithNoQuotesOrBracesAndNoSplitCharacters_ReturnsOneEntry()
        {
            // Arrange
            var input = "This has no quotes and no split characters.";

            // Act
            var result = input.SplitOnUnquotedCharacter(',').ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(input, result[0]);
        }

        [Test]
        public void SplitOnUnquotedCharacter_WithNoQuotesOrBraces_SplitsOnCharacter()
        {
            // Arrange
            var input = "This has no quotes, but has a split character.";

            // Act
            var result = input.SplitOnUnquotedCharacter(',').ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("This has no quotes", result[0]);
            Assert.AreEqual(" but has a split character.", result[1]);
        }

        [Test]
        public void SplitOnUnquotedCharacter_WithQuotes_SplitsOnCharacter()
        {
            // Arrange
            var input = "This has \"a quotation, with a split character\", but no braces.";

            // Act
            var result = input.SplitOnUnquotedCharacter(',').ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("This has \"a quotation, with a split character\"", result[0]);
            Assert.AreEqual(" but no braces.", result[1]);
        }

        [Test]
        public void SplitOnUnquotedCharacter_WithBraces_SplitsOnCharacter()
        {
            // Arrange
            var input = "This has no quotes, but {has braces, which have a split character}";

            // Act
            var result = input.SplitOnUnquotedCharacter(',').ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("This has no quotes", result[0]);
            Assert.AreEqual(" but {has braces, which have a split character}", result[1]);
        }

        [Test]
        public void SplitOnUnquotedCharacter_WithBracesAndQuotes_SplitsOnCharacter()
        {
            // Arrange
            var input = "This has \"some quotes, with a split characer\", and also has {has braces, which have a split character}";

            // Act
            var result = input.SplitOnUnquotedCharacter(',').ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("This has \"some quotes, with a split characer\"", result[0]);
            Assert.AreEqual(" and also has {has braces, which have a split character}", result[1]);
        }

        [TestCase("Not Here", "Key", "Value", ExpectedResult = "Not Here")]
        [TestCase("Key Here", "Key", "Value", ExpectedResult = "Value Here")]
        [TestCase("{Here is a} # Key", "Key", "Value", '#', ExpectedResult = "{Here is a} # Value")]
        public string Substitute_GivenKeyAndValue_ReturnsCorrectResult(string toSubstitute, string substitutionKey, string substitutionValue, char splitChar = ' ', string join = null)
        {
            // Arrange
            var substitition = new KeyValuePair<string, string>(substitutionKey, substitutionValue);

            // Act
            var result = toSubstitute.Substitute(substitition, splitChar, join);

            //Assert
            return result;
        }

        [Test]
        public void Substitute_GivenKeyAndValueAndAction_ReturnsCorrectResult()
        {
            // Arrange
            var toSubstitute = "{Here is a} # Key";
            var substitition = new KeyValuePair<string, string>("Key", "Value");

            // Act
            var result = toSubstitute.Substitute(substitition, '#', " ", x => x.Trim().RemoveFromStart('{', '"').RemoveFromEnd('}', '"'));

            //Assert
            Assert.AreEqual("Here is a Value", result);
        }
    }
}