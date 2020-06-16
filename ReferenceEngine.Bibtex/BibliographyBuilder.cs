using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Enumerations;
using ReferenceEngine.Bibtex.Extensions;
using ReferenceEngine.Bibtex.Manager;
using ReferenceEngine.Bibtex.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReferenceEngine.Bibtex
{
    /// <summary>
    /// Defines the methods required to build a bibliography style and write a styled .bbl file.
    /// </summary>
    public interface IBibliographyBuilder
    {
        /// <summary>
        /// The path to the .tex file to read.
        /// </summary>
        string TexFilePath { get; set; }

        /// <summary>
        /// The path to the .bib file to read.
        /// </summary>
        string BibFilePath { get; set; }

        /// <summary>
        /// The path to the style file to read.
        /// </summary>
        string StyleFilePath { get; set; }

        /// <summary>
        /// The bibliography style to use.
        /// </summary>
        BibliographyStyle BibliographyStyle { get; set; }

        /// <summary>
        /// Sets BibFilePath and StyleFilePath from Tex file.
        /// </summary>
        /// <returns>This bibliography builder instance for method chaining.</returns>
        BibliographyBuilder FromFile(string texFilePath);


        /// <summary>
        /// Builds the BibliographyStyle and applies it to the contents of the BibFilePath.
        /// </summary>
        /// <returns>This bibliography builder instance for method chaining.</returns>
        Bibliography Build();
    }

    /// <summary>
    /// An implementation of the IBibliographyBuilder used to build and write a styled .bbl file.
    /// </summary>
    public class BibliographyBuilder : IBibliographyBuilder
    {
        private readonly IFileManager _fileManager;
        private readonly IAuxParser _auxParser;
        private readonly IBibtexParser _bibParser;
        private readonly ILogger<BibliographyBuilder> _logger;
        private readonly ILoggerFactory _loggerFactory;

        private string _auxPath;
        private List<AuxEntry> _auxEntries;
        private bool _auxFileParsed = false;

        /// <summary>
        /// Constructs a new BibliographyBuilder.
        /// </summary>
        /// <param name="fileManager">FileManager used to check the existence of files.</param>
        /// <param name="auxParser">The AuxParser instance to use to read the .aux file.</param>
        /// <param name="bibParser">The BibtexParser instance to use to read the .bib file.</param>
        /// <param name="logger">The Logger used by this instance.</param>
        public BibliographyBuilder(IFileManager fileManager, IAuxParser auxParser, IBibtexParser bibParser, ILogger<BibliographyBuilder> logger)
        {
            _fileManager = fileManager;
            _auxParser = auxParser;
            _bibParser = bibParser;
            _logger = logger;
            _loggerFactory = new LoggerFactory();
        }

        /// <inheritdoc />
        public string TexFilePath { get; set; }

        /// <inheritdoc />
        public string BibFilePath { get; set; }

        /// <inheritdoc />
        public string StyleFilePath { get; set; }

        /// <inheritdoc />
        public BibliographyStyle BibliographyStyle { get; set; }

        /// <inheritdoc />
        public BibliographyBuilder FromFile(string texFilePath)
        {
            if (texFilePath == null)
            {
                throw new ArgumentNullException(nameof(texFilePath));
            }

            _fileManager.ThrowIfFileDoesNotExist(texFilePath);

            TexFilePath = texFilePath;
            var texDirectory = Path.GetDirectoryName(texFilePath);

            _auxPath = _fileManager.ReplaceExtension(TexFilePath, "aux");
            _auxEntries = _auxParser.ParseFile(_auxPath).ToList();
            _auxFileParsed = true;

            if (_auxEntries.TryGetFirst(x => x.Type == AuxEntryType.Bibdata, out var auxBibdataEntry))
            {
                BibFilePath = auxBibdataEntry.Key.EndsWith(".bib") ? Path.Combine(texDirectory, auxBibdataEntry.Key) : Path.Combine(texDirectory, $"{auxBibdataEntry.Key}.bib");
                _fileManager.ThrowIfFileDoesNotExist(BibFilePath);
            }

            if (_auxEntries.TryGetFirst(x => x.Type == AuxEntryType.Bibstyle, out var auxBibstyleEntry))
            {
                StyleFilePath = auxBibstyleEntry.Key.EndsWith(".style.json") ? Path.Combine(texDirectory, auxBibstyleEntry.Key) : Path.Combine(texDirectory, $"{auxBibstyleEntry.Key}.style.json");
                _fileManager.ThrowIfFileDoesNotExist(StyleFilePath);
            }

            return this;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException ">The TexFilePath must be provided prior to calling this method.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing the TexFilePath, BibFilePath and StyleFilePath must exist.</exception>
        /// <exception cref="FileNotFoundException">The files at TexFilePath, BibFilePath and StyleFilePath (if provided) must exist.</exception>
        public Bibliography Build()
        {
            _logger.LogTrace("Starting build of bibliography.");

            if (!_auxFileParsed)
            {
                if (TexFilePath == null)
                {
                    throw new InvalidOperationException("TexFilePath cannot be null!");
                }
                else
                {
                    _fileManager.ThrowIfFileDoesNotExist(TexFilePath);
                }

                if (BibFilePath == null)
                {
                    throw new InvalidOperationException("BibFilePath cannot be null!");
                }
                else
                {
                    _fileManager.ThrowIfFileDoesNotExist(BibFilePath);
                }

                if (BibliographyStyle == null)
                {
                    if (StyleFilePath == null)
                    {
                        throw new InvalidOperationException("Either the BibliographyStyle, or the StyleFilePath cannot be null!");
                    }
                    else
                    {
                        _fileManager.ThrowIfFileDoesNotExist(StyleFilePath);
                    }
                }

                _auxPath = _fileManager.ReplaceExtension(TexFilePath, "aux");
                _auxEntries = _auxParser.ParseFile(_auxPath).ToList();
            }

            try
            {
                BibliographyStyle = JsonConvert.DeserializeObject<BibliographyStyle>(_fileManager.ReadFileContents(StyleFilePath));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred parsing the style file.");
                throw;
            }

            var bibliography = new Bibliography(_fileManager, _loggerFactory.CreateLogger<Bibliography>())
            {
                TargetPath = _fileManager.ReplaceExtension(TexFilePath, "bbl"),
                TargetAuxPath = _auxPath
            };

            var bibtexDatabase = _bibParser.ParseFile(BibFilePath);
            foreach (var auxEntry in _auxEntries.Where(x => x.Type == AuxEntryType.Citation))
            {
                if (bibtexDatabase.Entries.TryGetSingle(bibtexEntry => bibtexEntry.CitationKey == auxEntry.Key, out var bibtexEntry))
                {
                    var style = BibliographyStyle.EntryStyles.SingleOrDefault(s => s.Type == bibtexEntry.EntryType) ?? EntryStyle.Default;
                    bibliography.Bibitems.Add(new Bibitem(auxEntry, bibtexEntry, style));
                }
            }

            _logger.LogTrace("Bibliography build completed.");

            return bibliography;
        }
    }
}