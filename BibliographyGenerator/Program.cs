using Bibtex;
using Bibtex.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace BibliographyGenerator
{
    public class Program
    {
        private static readonly string _baseDirectory;
        private static readonly IConfiguration _config;

        static Program()
        {
            _baseDirectory = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("bibgen.json", false, true).Build();
        }

        public static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var serviceProvider = ConfigureServices();

                var bibliographyBuilder = serviceProvider.GetRequiredService<IBibliographyBuilder>();
                bibliographyBuilder.TexFilePath = GetConfiguredPath("TexFilePath");
                bibliographyBuilder.BibFilePath = GetConfiguredPath("BibFilePath");
                bibliographyBuilder.StyleFilePath = GetConfiguredPath("StyleFilePath");

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
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddLogging(oLogBuilder =>
                {
                    oLogBuilder.ClearProviders();
                    oLogBuilder.AddNLog();
                })
                .AddBibliographyBuilder()
                .BuildServiceProvider();
        }

        private static string GetConfiguredPath(string pathName) => _config[$"Paths:{pathName}"].Replace("[CURRENT_DIRECTORY]", _baseDirectory);
    }
}