using Bibtex;
using Bibtex.Manager;
using Bibtex.Parser;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace Test.Bibtex
{
    [TestFixture]
    public class BibliographyBuilderTest
    {
        private readonly Mock<IFileManager> _mockFileManager = new Mock<IFileManager>();
        private readonly Mock<IAuxParser> _mockAuxParser = new Mock<IAuxParser>();
        private readonly Mock<IBibtexParser> _mockBibtexParser = new Mock<IBibtexParser>();
        private readonly Mock<ILogger<BibliographyBuilder>> _mockLogger = new Mock<ILogger<BibliographyBuilder>>();

        private BibliographyBuilder _bibliographyBuilder;

        [SetUp]
        public void SetUp()
        {
            _bibliographyBuilder = GetNewBibliographyBuilder();
        }

        [Test]
        public void Build_NullTexFilePath_ThrowsException()
        {
            // Arrange
            var bibliographyBuilder = GetNewBibliographyBuilder();
            bibliographyBuilder.BibFilePath = "";

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(() => bibliographyBuilder.Build());
            Assert.AreEqual("TexFilePath", ex.ParamName);
        }

        [Test]
        public void Build_NullBibFilePath_ThrowsException()
        {
            // Arrange
            var bibliographyBuilder = GetNewBibliographyBuilder();
            bibliographyBuilder.TexFilePath = "";

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(() => bibliographyBuilder.Build());
            Assert.AreEqual("BibFilePath", ex.ParamName);
        }

        private BibliographyBuilder GetNewBibliographyBuilder() => new BibliographyBuilder(_mockFileManager.Object, _mockAuxParser.Object, _mockBibtexParser.Object, _mockLogger.Object);
    }
}