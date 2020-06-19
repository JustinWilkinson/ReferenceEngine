using NUnit.Framework;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Abstractions.Entries;
using ReferenceEngine.Bibtex.Abstractions.Fields;
using ReferenceEngine.Bibtex.Enumerations;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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
            var style = GetStyle();

            // Act
            var result = entry.ApplyStyle(style);

            // Assert
            Assert.AreEqual("A Really Awesome Article by A. N. Author in \\emph{Some Really Good Journal}, 2020.", result);
        }

        [Test]
        public void GetStyledLabel_NoStyling_ReturnsIndex()
        {
            // Arrange
            var entry = GetEntry();
            var style = GetStyle();

            // Act
            var result = entry.GetStyledLabel(style, 1);

            // Assert
            Assert.AreEqual("1", result);
        }

        [Test]
        public void GetStyledLabel_WithLabelStyling_ReturnsTemplatedLabel()
        {
            // Arrange
            var entry = GetEntry();
            var style = GetStyle();
            style.Label = "{Index} - ({Year})";

            // Act
            var result = entry.GetStyledLabel(style, 1);

            // Assert
            Assert.AreEqual("1 - (2020)", result);
        }

        [Test]
        public void GetStyledLabel_WithLabelStyling_ReturnsTemplatedLabelCaseInsensitive()
        {
            // Arrange
            var entry = GetEntry();
            var style = GetStyle();
            style.Label = "{Index} - ({YEAR})";

            // Act
            var result = entry.GetStyledLabel(style, 1);

            // Assert
            Assert.AreEqual("1 - (2020)", result);
        }

        #region Private Helpers
        private BibtexEntry GetEntry()
        {
            return new BibtexEntry(EntryType.Article, "Article1", new Dictionary<string, string>
            {
                { "Author", "A. N. Author" },
                { "Title", "A Really Awesome Article" },
                { "Year", "2020" },
                { "Journal", "Some Really Good Journal" }
            });
        }

        private EntryStyle GetStyle()
        {
            return new EntryStyle
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
        }
        #endregion
    }
}