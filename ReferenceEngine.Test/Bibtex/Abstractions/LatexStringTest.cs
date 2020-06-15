using NUnit.Framework;
using ReferenceEngine.Bibtex.Abstractions;

namespace ReferenceEngine.Test.Bibtex.Abstractions
{
    [TestFixture]
    public class LatexStringTest
    {
        [Test]
        public void ToString_NotItalicOrBold_ReturnsValue()
        {
            // Arrange
            var latex = new LatexString("String Value");

            // Act
            var result = latex.ToString();

            // Assert
            Assert.AreEqual("String Value", result);
        }

        [Test]
        public void ToString_Italic_ReturnsItalicValue()
        {
            // Arrange
            var latex = new LatexString("String Value") { Italic = true };

            // Act
            var result = latex.ToString();

            // Assert
            Assert.AreEqual(@"\emph{String Value}", result);
        }

        [Test]
        public void ToString_Bold_ReturnsBoldValue()
        {
            // Arrange
            var latex = new LatexString("String Value") { Bold = true };

            // Act
            var result = latex.ToString();

            // Assert
            Assert.AreEqual(@"\textbf{String Value}", result);
        }

        [Test]
        public void ToString_Enquote_ReturnsBoldValue()
        {
            // Arrange
            var latex = new LatexString("String Value") { Enquote = true };

            // Act
            var result = latex.ToString();

            // Assert
            Assert.AreEqual(@"\enquote{String Value}", result);
        }

        [Test]
        public void ToString_ItalicAndBold_ReturnsItalicAndBoldValue()
        {
            // Arrange
            var latex = new LatexString("String Value") { Bold = true, Italic = true };

            // Act
            var result = latex.ToString();

            // Assert
            Assert.AreEqual(@"\textbf{\emph{String Value}}", result);
        }

        [Test]
        public void ToString_ItalicAndBoldAndEnquote_ReturnsItalicAndBoldValue()
        {
            // Arrange
            var latex = new LatexString("String Value") { Bold = true, Italic = true, Enquote = true };

            // Act
            var result = latex.ToString();

            // Assert
            Assert.AreEqual(@"\enquote{\textbf{\emph{String Value}}}", result);
        }
    }
}