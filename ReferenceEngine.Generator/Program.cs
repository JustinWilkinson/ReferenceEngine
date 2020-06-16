using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ReferenceEngine.Bibtex;
using ReferenceEngine.Bibtex.Extensions;
using System;
using System.IO;
using System.Reflection;
using ILogger = NLog.ILogger;

namespace ReferenceEngine.BibliographyGenerator
{
    public class Program
    {
        private static readonly string _baseDirectory;
        private static readonly ILogger _logger;

        static Program()
        {
            _baseDirectory = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            _logger = LogManager.GetCurrentClassLogger();
        }

        public static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    var path = Path.IsPathRooted(args[0]) ? args[0] : Path.Combine(_baseDirectory, args[0]);

                    _logger.Trace($"Bibliography generator started with path: '{path}'.");

                    var serviceProvider = ConfigureServices();
                    var bibliographyBuilder = serviceProvider.GetRequiredService<IBibliographyBuilder>();
                    var bibliography = bibliographyBuilder.FromFile(path).Build();
                    bibliography.Write();

                    _logger.Trace($"Bibliography successfully generated for file at path: '{path}'.");
                }
                else
                {
                    throw new ArgumentException("Expected a path to a tex file to be provided!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unexpected error occurred running the latex compiler!");
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
    }
}