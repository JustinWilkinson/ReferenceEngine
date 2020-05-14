using Bibtex;
using Bibtex.Manager;
using Bibtex.Parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;

namespace LatexCompiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var serviceProvider = ConfigureServices();

                var bibliographyBuilder = serviceProvider.GetRequiredService<IBibliographyBuilder>();
                bibliographyBuilder.TexFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Latex", "document.tex");
                bibliographyBuilder.BibFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Latex", "lib.bib");

                bibliographyBuilder.Build();
                bibliographyBuilder.Write();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An unexpected error occurred running the latex compiler!");
            }
            finally
            {
                LogManager.Shutdown();
                Console.ReadKey();
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            IConfiguration oConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("LatexCompiler.json", false, true)
                .Build();

            return new ServiceCollection()
                .AddLogging(oLogBuilder =>
                {
                    oLogBuilder.ClearProviders();
                    oLogBuilder.AddNLog();
                })
                .AddSingleton<IFileManager, FileManager>()
                .AddSingleton<IAuxParser, AuxParser>()
                .AddSingleton<IBibtexParser, BibtexParser>()
                .AddSingleton<IBibliographyBuilder, BibliographyBuilder>()
                .BuildServiceProvider();
        }
    }
}