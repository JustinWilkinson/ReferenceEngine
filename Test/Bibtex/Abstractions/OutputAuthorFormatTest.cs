using Bibtex.Abstractions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Test.Bibtex.Abstractions
{
    [TestFixture]
    public class OutputAuthorFormatTest
    {
        [Test]
        public void GetFormattedIndividualAuthors_NullAuthorField_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new OutputAuthorFormat().GetFormattedIndividualAuthors(null).Any());
        }

        [Test]
        public void GetFormattedIndividualAuthors_ValidAuthorInAuthorField_ProcessesCorrectly()
        {
            // Arrange
            var input = "Anthony N. Author";

            // Act
            var result = new OutputAuthorFormat().GetFormattedIndividualAuthors(input).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Anthony Author", result[0]);
        }

        [TestCase(1, ExpectedResult = "A. Author")]
        [TestCase(3, ExpectedResult = "Ant. Author")]
        [TestCase(null, ExpectedResult = "Anthony Author")]
        [TestCase(0, ExpectedResult = "Anthony Author")]
        [TestCase(-1, ExpectedResult = "Anthony Author")]
        public string GetFormattedIndividualAuthors_AbreviateCharacters_AbbreviatesCorrectly(int? abbreviateChars)
        {
            // Arrange
            var input = "Anthony Author";
            var format = new OutputAuthorFormat { AbbreviateFirstNameCharacters = abbreviateChars };

            // Act
            var result = format.GetFormattedIndividualAuthors(input).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            return result[0];
        }

        [Test]
        public void GetFormattedAuthors_NullAuthorField_ReturnsNull()
        {
            Assert.AreEqual(null, new OutputAuthorFormat().FormatAuthorField(null));
        }
    }
}