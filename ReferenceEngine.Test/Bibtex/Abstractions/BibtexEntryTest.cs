using NUnit.Framework;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Abstractions.Entries;
using ReferenceEngine.Bibtex.Abstractions.Fields;
using ReferenceEngine.Bibtex.Enumerations;
using System;
using System.Collections.Generic;

namespace ReferenceEngine.Test.Bibtex.Abstractions
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
                    new EntryAuthorField { Format = new OutputAuthorFormat() },
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