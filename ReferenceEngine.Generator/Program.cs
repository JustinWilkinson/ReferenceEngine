﻿using Bibtex;
using Bibtex.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using ILogger = NLog.ILogger;

namespace BibliographyGenerator
{
    public class Program
    {
        private static readonly string _baseDirectory;
        private static readonly IConfiguration _config;
        private static readonly ILogger _logger;

        static Program()
        {
            _baseDirectory = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("bibgen.json", false, true).Build();
            _logger = LogManager.GetCurrentClassLogger();
        }

        public static void Main(string[] args)
        {
            try
            {
                _logger.Trace("Bibliography generaor started.");

                var serviceProvider = ConfigureServices();

                var bibliographyBuilder = serviceProvider.GetRequiredService<IBibliographyBuilder>();
                bibliographyBuilder.TexFilePath = GetConfiguredPath("TexFilePath");
                bibliographyBuilder.BibFilePath = GetConfiguredPath("BibFilePath");
                bibliographyBuilder.StyleFilePath = GetConfiguredPath("StyleFilePath");
                
                bibliographyBuilder.Build();
                bibliographyBuilder.Write();

                _logger.Trace("Bibliography successfully generated.");
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

        private static string GetConfiguredPath(string pathName) => _config[$"Paths:{pathName}"].Replace("[CURRENT_DIRECTORY]", _baseDirectory);
    }
}