using Bibtex.Abstractions;
using Bibtex.Abstractions.Entries;
using Bibtex.Abstractions.Fields;
using Bibtex.Enumerations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Test.Bibtex.Abstractions
{
    [TestFixture]
    public class BibtexEntryTest
    {
        [Test]
        public void ApplyStyle_NullEntryStyle_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => GetEntry().ApplyStyle(null));
        }

        [Test]
        public void ApplyStyle_ValidEntryStyle_FormatsOutputSuccessfully()
        {
            // Arrange
            var entry = GetEntry();
            var style = new EntryStyle
            {
                Fields = new List<Field>
                {
                    new EntryField { Value = "Title" },
                    new ConstantField { Value = "by" },
                    new EntryAuthorField { Format = OutputAuthorFormat.Default },
                    new ConstantField { Value = "in" },
                    new EntryField { Value = "Journal", Italic = true, Suffix = "," },
                    new EntryField { Value = "Year", Suffix = "." }
                }
            };

            // Act
            var result = entry.ApplyStyle(style);

            // Assert
            Assert.AreEqual("A Really Awesome Article by A. N. Author in \\emph{Some Really Good Journal}, 2020.", result);
        }

        public BibtexEntry GetEntry()
        {
            return new BibtexEntry(EntryType.Article, "Article1", new Dictionary<string, string> 
            {
                { "Author", "A. N. Author" },
                { "Title", "A Really Awesome Article" },
                { "Year", "2020" },
                { "Journal", "Some Really Good Journal" }
            });
        }
    }
}