using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ReferenceEngine.Bibtex;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Manager;
using ReferenceEngine.Bibtex.Parser;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReferenceEngine.Test.Bibtex
{
    [TestFixture]
    public class BibliographyBuilderTest
    {
        private const string BibPath = "BibFilePath";
        private const string TexPath = "TexFilePath";
        private const string StylePath = "StyleFilePath";
        private const string AuxPath = "AuxFilePath";
        private const string BblPath = "BblPath";

        private readonly Mock<IFileManager> _mockFileManager = new Mock<IFileManager>();
        private readonly Mock<IAuxParser> _mockAuxParser = new Mock<IAuxParser>();
        private readonly Mock<IBibtexParser> _mockBibtexParser = new Mock<IBibtexParser>();
        private readonly Mock<ILogger<BibliographyBuilder>> _mockLogger = new Mock<ILogger<BibliographyBuilder>>();

        [SetUp]
        public void SetUp()
        {
            _mockFileManager.Reset();
            _mockAuxParser.Reset();
            _mockBibtexParser.Reset();
            _mockLogger.Reset();
        }

        [Test]
        public void Build_NullTexFilePath_ThrowsException()
        {
            // Arrange
            var bibliographyBuilder = GetNewBibliographyBuilder();
            bibliographyBuilder.BibFilePath = BibPath;

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(() => bibliographyBuilder.Build());
            Assert.AreEqual("TexFilePath", ex.ParamName);
        }

        [Test]
        public void Build_NullBibFilePath_ThrowsException()
        {
            // Arrange
            var bibliographyBuilder = GetNewBibliographyBuilder();
            bibliographyBuilder.TexFilePath = TexPath;

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(() => bibliographyBuilder.Build());
            Assert.AreEqual("BibFilePath", ex.ParamName);
        }

        [Test]
        public void Build_NullBibliographyStyleAndNullStyleFilePath_ThrowsException()
        {
            // Arrange
            var bibliographyBuilder = GetNewBibliographyBuilder();
            bibliographyBuilder.BibFilePath = BibPath;
            bibliographyBuilder.TexFilePath = TexPath;

            // Act/Assert
            var ex = Assert.Throws<ArgumentNullException>(() => bibliographyBuilder.Build());
            Assert.AreEqual("BibliographyStyle", ex.ParamName);
        }

        [Test]
        public void Build_ValidParameters_MakesCorrectMethodCalls()
        {
            // Arrange
            var bibliographyBuilder = GetNewBibliographyBuilder();
            bibliographyBuilder.BibFilePath = BibPath;
            bibliographyBuilder.TexFilePath = TexPath;
            bibliographyBuilder.StyleFilePath = StylePath;
            _mockFileManager.Setup(x => x.ReadFileContents(It.IsAny<string>())).Returns("{ Id: 1, Name: \"Some Name\", EntryStyles: [] }");
            _mockFileManager.Setup(x => x.ReplaceExtension(It.IsAny<string>(), It.IsAny<string>())).Returns(AuxPath);
            _mockAuxParser.Setup(x => x.ParseFile(It.IsAny<string>())).Returns(new List<AuxEntry>());


            // Act
            bibliographyBuilder.Build();

            // Assert
            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(TexPath), Times.Once);
            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(BibPath), Times.Once);
            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(StylePath), Times.Once);
            _mockFileManager.Verify(x => x.ReadFileContents(StylePath), Times.Once);
            _mockFileManager.Verify(x => x.ReplaceExtension(TexPath, "aux"), Times.Once);
            _mockAuxParser.Verify(x => x.ParseFile(AuxPath), Times.Once);
            _mockBibtexParser.Verify(x => x.ParseFile(BibPath), Times.Once);
            _mockFileManager.VerifyNoOtherCalls();
            _mockAuxParser.VerifyNoOtherCalls();
            _mockBibtexParser.VerifyNoOtherCalls();
        }

        [Test]
        public void Write_WritesContents_CallsCorrectMethods()
        {
            // Arrange
            var bibliographyBuilder = GetNewBibliographyBuilder();
            bibliographyBuilder.TexFilePath = TexPath;
            _mockFileManager.Setup(x => x.ReplaceExtension(TexPath, "bbl")).Returns(BblPath);

            // Act
            bibliographyBuilder.Write();

            // Assert
            _mockFileManager.Verify(x => x.ReplaceExtension(TexPath, "bbl"), Times.Once);
            _mockFileManager.Verify(x => x.DeleteIfExists(BblPath), Times.Once);
            _mockFileManager.Verify(x => x.WriteStream(BblPath, It.IsAny<Action<StreamWriter>>()), Times.Once);
            _mockFileManager.VerifyNoOtherCalls();
        }

        private BibliographyBuilder GetNewBibliographyBuilder() => new BibliographyBuilder(_mockFileManager.Object, _mockAuxParser.Object, _mockBibtexParser.Object, _mockLogger.Object);
    }
}