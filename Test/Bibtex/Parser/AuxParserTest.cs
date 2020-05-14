using Bibtex.Enumerations;
using Bibtex.Manager;
using Bibtex.Parser;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Test.Bibtex.Parser
{
    [TestFixture]
    public class AuxParserTest
    {
        private readonly AuxParser _parser = new AuxParser(new Mock<IFileManager>().Object, new Mock<ILogger<AuxParser>>().Object);

        [TestCase(@"\relax", ExpectedResult = AuxEntryType.Relax)]
        [TestCase(@"\bibstyle", ExpectedResult = AuxEntryType.Bibstyle)]
        [TestCase(@"\citation", ExpectedResult = AuxEntryType.Citation)]
        [TestCase(@"\bibdata", ExpectedResult = AuxEntryType.Bibdata)]
        [TestCase(@"\bibcite", ExpectedResult = AuxEntryType.Bibcite)]
        [TestCase(@"Nonsense", ExpectedResult = null)]
        public AuxEntryType? ParseEntry_Line_ReturnsCorrectType(string line)
        {
            return _parser.ParseEntry(line)?.Type;
        }

        [Test]
        public void ParseEntry_BibCiteEntry_ReturnsCorrectKeyAndLabel()
        {
            // Arrange
            var line = @"\bibcite{key}{label}";

            // Act
            var entry = _parser.ParseEntry(line);

            // Assert
            Assert.AreEqual(AuxEntryType.Bibcite, entry.Type);
            Assert.AreEqual("key", entry.Key);
            Assert.AreEqual("label", entry.Label);
        }
    }
}