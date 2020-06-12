using Bibtex.Manager;
using Bibtex.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace Bibtex.Extensions
{
    /// <summary>
    /// Contains extension methods for adding services to a collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a BibliographyBuilder service with all its dependencies to the provided service collection.
        /// </summary>
        /// <param name="services">Service collection to add to.</param>
        /// <returns>A reference to this instance after the operation has completed, for method chaining.</returns>
        public static IServiceCollection AddBibliographyBuilder(this IServiceCollection services)
        {
            return services.AddSingleton<IFileManager, FileManager>()
                .AddSingleton<IAuxParser, AuxParser>()
                .AddSingleton<IBibtexParser, BibtexParser>()
                .AddSingleton<IBibliographyBuilder, BibliographyBuilder>();
        }
    }
}