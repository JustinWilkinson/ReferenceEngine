using Bibtex.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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

        [Test]
        public void TryGetSingle_NoPredicateWithNoValueClass_ReturnsFalseWithDefaultOutValue()
        {
            // Arrange
            var source = new List<string>();

            // Act
            var result = source.TryGetSingle(out var str);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(str);
        }

        [Test]
        public void TryGetSingle_NoPredicateWithNoValueStruct_ReturnsFalseWithDefaultOutValue()
        {
            // Arrange
            var source = new List<int>();

            // Act
            var result = source.TryGetSingle(out var integer);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, integer);
        }

        [Test]
        public void TryGetSingle_NoPredicateWithValue_ReturnsTrueWithCorrectOutValue()
        {
            // Arrange
            var source = new[] { "Hello" };

            // Act
            var result = source.TryGetSingle(out var str);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Hello", str);
        }

        [Test]
        public void TryGetSingle_WithPredicateAndNoMatch_ReturnsFalseWithDefaultOutValue()
        {
            // Arrange
            var source = new[] { "Hello", "World" };

            // Act
            var result = source.TryGetSingle(s => s == "Not in source!", out var str);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(str);
        }

        [Test]
        public void TryGetSingle_WithPredicateAndNoMatch_ReturnsTrueWithCorrectOutValue()
        {
            // Arrange
            var source = new[] { "Hello", "World" };

            // Act
            var result = source.TryGetSingle(s => s == "Hello", out var str);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Hello", str);
        }
    }
}