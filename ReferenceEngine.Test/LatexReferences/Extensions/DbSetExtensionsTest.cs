using Bibtex.Abstractions;
using LatexReferences.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;
using Test.LatexReferences.Helpers;

namespace Test.LatexReferences.Extensions
{
    [TestFixture]
    public class DbSetExtensionsTest
    {
        private TestDb _db;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _db = new TestDb();
            _db.EntryStyles.Add(new EntryStyle { Id = 1, Name = "Name 1" });
            _db.SaveChanges();
        }

        [Test]
        public async Task TryFindAsync_FindsResult_ReturnsTrueAndValue()
        {
            // Act
            var (found, value) = await _db.EntryStyles.TryFindAsync(1);

            // Assert
            Assert.AreEqual(true, found);
            Assert.AreEqual(1, value.Id);
            Assert.AreEqual("Name 1", value.Name);
        }

        [Test]
        public async Task TryFindAsync_ResultNotPresent_ReturnsFalseWithNullValue()
        {
            // Act
            var (found, value) = await _db.EntryStyles.TryFindAsync(2);

            // Assert
            Assert.AreEqual(false, found);
            Assert.IsNull(value);
        }
    }
}