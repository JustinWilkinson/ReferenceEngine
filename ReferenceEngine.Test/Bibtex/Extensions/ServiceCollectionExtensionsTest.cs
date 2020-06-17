using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ReferenceEngine.Bibtex;
using ReferenceEngine.Bibtex.Extensions;

namespace ReferenceEngine.Test.Bibtex.Extensions
{
    [TestFixture]
    public class ServiceCollectionExtensionsTest
    {
        [Test]
        public void AddBibliographyBuilder_OnceCalled_CanInstantiateService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var serviceProvider = services.AddBibliographyBuilder().BuildServiceProvider();

            // Assert
            Assert.IsInstanceOf<IBibliographyBuilder>(serviceProvider.GetRequiredService<IBibliographyBuilder>());
        }
    }
}