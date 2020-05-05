using Bibtex;
using Bibtex.Enumerations;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace Test.Bibtex
{
    [TestFixture]
    public class BibtexParserTest
    {
        private readonly BibtexParser _parser = new BibtexParser(new Mock<ILogger<BibtexParser>>().Object);

        private const string BookEntry = @"@Book{steward03,
              author =	 { Martha Steward },
              title =	 {Cooking behind bars},
              publisher =	 {Culinary Expert Series},
              year =	 2003
            }";

        private const string SampleDatabase = @"@string{ text = ""replacement text"" }
            @comment{ 
                this will be ignored during compilation 
            }
            @preamble {""This bibliography was generated on \today""}
            @article{one,
                title = {Good quantum error-correcting codes exist},
                author = {Calderbank, A. R. and Shor, Peter W.},
                journal = {Phys. Rev. A},
                volume = {54},
                number = {2},
                pages = {1098--1105},
                year = {1996}
            }
            @misc{two,
                author = {M. A. Nielsen and J. Kempe},
                year = 2001,
                title = {Separable states are more disordered globally than locally},
            }
            @book{three,
                author = {Albert W. Marshall and Ingram Olkin},
                title = {Inequalities: theory of majorization and its applications},
                year = 1979,
                publisher = {Academic Press},
                address = {New York}
            }";

        [Test]
        public void GetEntryType_StreamWithBibtexContent_GetsContentType()
        {
            // Arrange
            using var stream = new MemoryStream(Encoding.ASCII.GetBytes(BookEntry));
            using var sr = new StreamReader(stream);

            // Act
            var result = _parser.GetEntryType(sr);

            // Assert
            Assert.AreEqual(EntryType.Book, result);
        }

        [Test]
        public void GetEntryContent_StreamWithBibtexContent_GetsContentType()
        {
            // Arrange
            using var stream = new MemoryStream(Encoding.ASCII.GetBytes(BookEntry.Replace("@Book", "")));
            using var sr = new StreamReader(stream);

            // Act
            var result = _parser.GetEntryContent(EntryType.Book, sr);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("steward03", result.CitationKey);
            Assert.AreEqual("Martha Steward", result.Author);
            Assert.AreEqual("Cooking behind bars", result.Title);
            Assert.AreEqual("Culinary Expert Series", result.Publisher);
            Assert.AreEqual("2003", result.Year);
        }

        [Test]
        public void ParseString_StreamWithBibtexContent_GetsContentType()
        {
            // Arrange 
            var databaseName = "DatabaseName";

            // Act
            var result = _parser.ParseString(databaseName, SampleDatabase);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(databaseName, result.Name);
            
            // Comment tags.
            Assert.AreEqual(1, result.Comments.Count);
            Assert.AreEqual("this will be ignored during compilation", result.Comments[0].Text);

            // Preamble tags.
            Assert.AreEqual(1, result.Preambles.Count);
            Assert.AreEqual("This bibliography was generated on \\today", result.Preambles[0].Content);

            // String tags.
            Assert.AreEqual(1, result.Strings.Count);
            Assert.AreEqual("text", result.Strings[0].Content.Key);
            Assert.AreEqual("replacement text", result.Strings[0].Content.Value);

            // Entries
            Assert.AreEqual(3, result.Entries.Count);
            
            var firstEntry = result.Entries[0];
            Assert.IsNotNull(firstEntry);
            Assert.AreEqual(EntryType.Article, firstEntry.EntryType);
            Assert.AreEqual("one", firstEntry.CitationKey);
            Assert.AreEqual("Calderbank, A. R. and Shor, Peter W.", firstEntry.Author);
            Assert.AreEqual("Good quantum error-correcting codes exist", firstEntry.Title);
            Assert.AreEqual("54", firstEntry.Volume);
            Assert.AreEqual("2", firstEntry.Number);
            Assert.AreEqual("1098--1105", firstEntry.Pages);
            Assert.AreEqual("1996", firstEntry.Year);

            var secondEntry = result.Entries[1];
            Assert.IsNotNull(secondEntry);
            Assert.AreEqual(EntryType.Misc, secondEntry.EntryType);
            Assert.AreEqual("two", secondEntry.CitationKey);
            Assert.AreEqual("M. A. Nielsen and J. Kempe", secondEntry.Author);
            Assert.AreEqual("Separable states are more disordered globally than locally", secondEntry.Title);
            Assert.AreEqual("2001", secondEntry.Year);

            var thirdEntry = result.Entries[2];
            Assert.IsNotNull(thirdEntry);
            Assert.AreEqual(EntryType.Book, thirdEntry.EntryType);
            Assert.AreEqual("three", thirdEntry.CitationKey);
            Assert.AreEqual("Albert W. Marshall and Ingram Olkin", thirdEntry.Author);
            Assert.AreEqual("Inequalities: theory of majorization and its applications", thirdEntry.Title);
            Assert.AreEqual("1979", thirdEntry.Year);
            Assert.AreEqual("Academic Press", thirdEntry.Publisher);
            Assert.AreEqual("New York", thirdEntry.Address);
        }
    }
}