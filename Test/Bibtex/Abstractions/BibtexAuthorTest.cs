using Bibtex.Abstractions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Test.Bibtex.Abstractions
{
    [TestFixture]
    public class BibtexAuthorTest
    {
        [Test]
        public void FromString_GivenNullString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => BibtexAuthor.FromString(null));
        }

        [Test]
        public void FromString_GivenEmptyString_ThrowsFormatException()
        {
            Assert.Throws<ArgumentException>(() => BibtexAuthor.FromString(""));
        }

        [Test]
        public void FromString_GivenWhitespaceString_ThrowsFormatException()
        {
            Assert.Throws<ArgumentException>(() => BibtexAuthor.FromString("  "));
        }

        [Test]
        public void FromString_GivenFirstLastAuthorString_BuildsAuthorCorrectly()
        {
            // Arrange
            var input = "Anthony N. Author";

            // Act
            var author = BibtexAuthor.FromString(input);

            // Assert
            Assert.AreEqual("Anthony", author.FirstName);
            Assert.AreEqual(1, author.MiddleNames.Count());
            Assert.AreEqual("N.", author.MiddleNames.First());
            Assert.AreEqual("Author", author.LastName);
            Assert.AreEqual(null, author.Suffix);
        }

        [Test]
        public void FromString_GivenLastFirstAuthorString_BuildsAuthorCorrectly()
        {
            // Arrange
            var input = "Author, Anthony N.";

            // Act
            var author = BibtexAuthor.FromString(input);

            // Assert
            Assert.AreEqual("Anthony", author.FirstName);
            Assert.AreEqual(1, author.MiddleNames.Count());
            Assert.AreEqual("N.", author.MiddleNames.First());
            Assert.AreEqual("Author", author.LastName);
            Assert.AreEqual(null, author.Suffix);
        }


        [Test]
        public void FromString_GivenLastSuffixFirstAuthorString_BuildsAuthorCorrectly()
        {
            // Arrange
            var input = "Author, Jr, Anthony N.";

            // Act
            var author = BibtexAuthor.FromString(input);

            // Assert
            Assert.AreEqual("Anthony", author.FirstName);
            Assert.AreEqual(1, author.MiddleNames.Count());
            Assert.AreEqual("N.", author.MiddleNames.First());
            Assert.AreEqual("Author", author.LastName);
            Assert.AreEqual("Jr", author.Suffix);
        }

        [Test]
        public void FromString_GivenAuthorStringWithTooManyComponents_ThrowsExceptionWithCorrectMessage()
        {
            var ex = Assert.Throws<FormatException>(() => BibtexAuthor.FromString("This, has, too, many, parts"));
            Assert.AreEqual("Unrecognized BibTeX format for author field with value: 'This, has, too, many, parts'", ex.Message);
        }
    }
}