using Bibtex.Extensions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Test.Bibtex.Extensions
{
    [TestFixture]
    public class LinqExtensionsTest
    {
        [Test]
        public void HasContent_Null_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> nullEnumerable = null;

            // Act
            var result = nullEnumerable.HasContent();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasContent_Emtpy_ReturnsFalse()
        {
            // Arrange
            IEnumerable<string> empty = new List<string>();

            // Act
            var result = empty.HasContent();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasContent_WithContent_ReturnsTrue()
        {
            // Arrange
            IEnumerable<string> enumerable = new[] { "One", "Two", "Three" };

            // Act
            var result = enumerable.HasContent();

            // Assert
            Assert.IsTrue(result);
        }
    }
}