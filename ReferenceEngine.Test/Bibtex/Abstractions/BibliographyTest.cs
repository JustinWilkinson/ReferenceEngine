using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Manager;
using System;
using System.IO;

namespace ReferenceEngine.Test.Bibtex.Abstractions
{
    [TestFixture]
    public class BibliographyTest
    {
        private const string BblPath = "BblPath";

        private readonly Mock<IFileManager> _mockFileManager = new Mock<IFileManager>();
        private readonly Mock<ILogger<Bibliography>> _mockLogger = new Mock<ILogger<Bibliography>>();

        [Test]
        public void Write_WritesContents_CallsCorrectMethods()
        {
            // Arrange
            var bibliography = new Bibliography(_mockFileManager.Object, _mockLogger.Object) { TargetPath = BblPath };

            // Act
            bibliography.Write();

            // Assert
            _mockFileManager.Verify(x => x.DeleteIfExists(BblPath), Times.Once);
            _mockFileManager.Verify(x => x.WriteStream(BblPath, It.IsAny<Action<StreamWriter>>()), Times.Once);
            _mockFileManager.VerifyNoOtherCalls();
        }
    }
}