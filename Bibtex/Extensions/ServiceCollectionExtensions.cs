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
        public static IServiceCollection AddBibliographyBuilder(this IServiceCollection services)
        {
            return services.AddSingleton<IFileManager, FileManager>()
                .AddSingleton<IAuxParser, AuxParser>()
                .AddSingleton<IBibtexParser, BibtexParser>()
                .AddSingleton<IBibliographyBuilder, BibliographyBuilder>();
        }
    }
}