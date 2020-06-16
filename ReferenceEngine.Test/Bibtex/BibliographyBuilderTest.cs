using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ReferenceEngine.Bibtex;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Enumerations;
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
        private const string BibPath = @"C:\SomeFolder\lib.bib";
        private const string TexPath = @"C:\SomeFolder\document.tex";
        private const string StylePath = @"C:\SomeFolder\custom.style.json";
        private const string AuxPath = @"C:\SomeFolder\document.aux";

        private readonly Mock<IFileManager> _mockFileManager = new Mock<IFileManager>();
        private readonly Mock<IAuxParser> _mockAuxParser = new Mock<IAuxParser>();
        private readonly Mock<IBibtexParser> _mockBibtexParser = new Mock<IBibtexParser>();
        private readonly Mock<ILogger<BibliographyBuilder>> _mockLogger = new Mock<ILogger<BibliographyBuilder>>();

        private IBibliographyBuilder _bibliographyBuilder;

        [SetUp]
        public void SetUp()
        {
            _mockFileManager.Reset();
            _mockAuxParser.Reset();
            _mockBibtexParser.Reset();
            _mockLogger.Reset();
            _bibliographyBuilder = GetNewBibliographyBuilder();
        }

        [Test]
        public void Build_NullTexFilePath_ThrowsException()
        {
            // Arrange
            _bibliographyBuilder.BibFilePath = BibPath;
            _bibliographyBuilder.StyleFilePath = StylePath;

            // Act/Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _bibliographyBuilder.Build());
            Assert.AreEqual("TexFilePath cannot be null!", ex.Message);
        }

        [Test]
        public void Build_NullBibFilePath_ThrowsException()
        {
            // Arrange
            _bibliographyBuilder.TexFilePath = TexPath;
            _bibliographyBuilder.StyleFilePath = StylePath;

            // Act/Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _bibliographyBuilder.Build());
            Assert.AreEqual("BibFilePath cannot be null!", ex.Message);
        }

        [Test]
        public void Build_NullBibliographyStyleAndNullStyleFilePath_ThrowsException()
        {
            // Arrange
            _bibliographyBuilder.BibFilePath = BibPath;
            _bibliographyBuilder.TexFilePath = TexPath;

            // Act/Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _bibliographyBuilder.Build());
            Assert.AreEqual("Either the BibliographyStyle, or the StyleFilePath cannot be null!", ex.Message);
        }

        [Test]
        public void Build_ValidParameters_MakesCorrectMethodCalls()
        {
            // Arrange
            _bibliographyBuilder.BibFilePath = BibPath;
            _bibliographyBuilder.TexFilePath = TexPath;
            _bibliographyBuilder.StyleFilePath = StylePath;
            _mockFileManager.Setup(x => x.ReadFileContents(It.IsAny<string>())).Returns("{ Id: 1, Name: \"Some Name\", EntryStyles: [] }");
            _mockFileManager.Setup(x => x.ReplaceExtension(It.IsAny<string>(), It.IsAny<string>())).Returns(AuxPath);
            _mockAuxParser.Setup(x => x.ParseFile(It.IsAny<string>())).Returns(new List<AuxEntry>());

            // Act
            var bibliography = _bibliographyBuilder.Build();

            // Assert
            Assert.IsNotNull(bibliography);
            Assert.IsInstanceOf<Bibliography>(bibliography);

            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(TexPath), Times.Once);
            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(BibPath), Times.Once);
            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(StylePath), Times.Once);

            _mockFileManager.Verify(x => x.ReplaceExtension(TexPath, "aux"), Times.Once);
            _mockAuxParser.Verify(x => x.ParseFile(AuxPath), Times.Once);

            _mockFileManager.Verify(x => x.ReadFileContents(StylePath), Times.Once);

            _mockFileManager.Verify(x => x.ReplaceExtension(TexPath, "bbl"));
            _mockBibtexParser.Verify(x => x.ParseFile(BibPath), Times.Once);

            _mockFileManager.VerifyNoOtherCalls();
            _mockAuxParser.VerifyNoOtherCalls();
            _mockBibtexParser.VerifyNoOtherCalls();
        }

        [Test]
        public void FromFileThenBuild_ValidParameters_MakesCorrectMethodCalls()
        {
            // Arrange
            _mockFileManager.Setup(x => x.ReadFileContents(It.IsAny<string>())).Returns("{ Id: 1, Name: \"Some Name\", EntryStyles: [] }");
            _mockFileManager.Setup(x => x.ReplaceExtension(It.IsAny<string>(), It.IsAny<string>())).Returns(AuxPath);
            _mockAuxParser.Setup(x => x.ParseFile(It.IsAny<string>())).Returns(new List<AuxEntry>
            {
                new AuxEntry(AuxEntryType.Bibdata) { Key = BibPath },
                new AuxEntry(AuxEntryType.Bibstyle) { Key = StylePath }
            });

            // Act
            var bibliography = _bibliographyBuilder.FromFile(TexPath).Build();

            // Assert
            Assert.IsNotNull(bibliography);
            Assert.IsInstanceOf<Bibliography>(bibliography);

            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(TexPath), Times.Once);
            _mockFileManager.Verify(x => x.ReplaceExtension(TexPath, "aux"), Times.Once);
            _mockAuxParser.Verify(x => x.ParseFile(AuxPath), Times.Once);

            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(BibPath), Times.Once);
            _mockFileManager.Verify(x => x.ThrowIfFileDoesNotExist(StylePath), Times.Once);

            _mockFileManager.Verify(x => x.ReadFileContents(StylePath), Times.Once);

            _mockFileManager.Verify(x => x.ReplaceExtension(TexPath, "bbl"));
            _mockBibtexParser.Verify(x => x.ParseFile(BibPath), Times.Once);

            _mockFileManager.VerifyNoOtherCalls();
            _mockAuxParser.VerifyNoOtherCalls();
            _mockBibtexParser.VerifyNoOtherCalls();
        }

        private BibliographyBuilder GetNewBibliographyBuilder() => new BibliographyBuilder(_mockFileManager.Object, _mockAuxParser.Object, _mockBibtexParser.Object, _mockLogger.Object);
    }
}