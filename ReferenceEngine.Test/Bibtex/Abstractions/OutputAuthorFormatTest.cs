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
            Assert.AreEqual("Anthony N. Author", result[0]);
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

        [TestCase("Anthony Author", true, ExpectedResult = "Author, Anthony")]
        [TestCase("Author, Anthony", true, ExpectedResult = "Author, Anthony")]
        [TestCase("Anthony Author", false, ExpectedResult = "Anthony Author")]
        [TestCase("Author, Anthony", false, ExpectedResult = "Anthony Author")]
        public string GetFormattedIndividualAuthors_LastNameFirst_FormatsCorrectly(string input, bool lastNameFirst)
        {
            // Arrange
            var format = new OutputAuthorFormat { LastNameFirst = lastNameFirst };

            // Act
            var result = format.GetFormattedIndividualAuthors(input).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            return result[0];
        }

        [TestCase("A. N. Author", true, ExpectedResult = "A. N. Author")]
        [TestCase("A. N. Author", false, ExpectedResult = "A. Author")]
        [TestCase("Author, A. N.", true, ExpectedResult = "A. N. Author")]
        [TestCase("Author, A. N.", false, ExpectedResult = "A. Author")]
        public string GetFormattedIndividualAuthors_IncludeMiddleNames_FormatsCorrectly(string input, bool includeMiddleNames)
        {
            // Arrange
            var format = new OutputAuthorFormat { IncludeMiddleNames = includeMiddleNames };

            // Act
            var result = format.GetFormattedIndividualAuthors(input).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            return result[0];
        }


        [TestCase("Author, Jr., Anthony", true, ExpectedResult = "Anthony Author Jr.")]
        [TestCase("Author, Jr., Anthony", false, ExpectedResult = "Anthony Author")]
        public string GetFormattedIndividualAuthors_IncludeSuffix_FormatsCorrectly(string input, bool includeSuffix)
        {
            // Arrange
            var format = new OutputAuthorFormat { IncludeSuffix = includeSuffix };

            // Act
            var result = format.GetFormattedIndividualAuthors(input).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            return result[0];
        }

        [Test]
        public void FormatAuthorField_NullAuthorField_ReturnsNull()
        {
            Assert.AreEqual(null, new OutputAuthorFormat().FormatAuthorField(null));
        }

        [Test]
        public void FormatAuthorField_AuthorField_ReturnsFormattedString()
        {
            // Arrange
            var input = "A. N. Author and Author, Anne Other and A. Final Author";
            var format = new OutputAuthorFormat { IncludeMiddleNames = true, Delimiter = ',', FinalDelimiter = "and", NumberOfNamedAuthors = 3 };

            // Act
            var result = format.FormatAuthorField(input);

            // Assert
            Assert.AreEqual("A. N. Author, Anne Other Author and A. Final Author", result);
        }
    }
}