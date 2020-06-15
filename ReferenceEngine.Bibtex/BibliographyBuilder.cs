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
        BibliographyBuilder Build();

        /// <summary>
        /// Writes the bibliography to a .bbl file.
        /// </summary>
        /// <returns>This bibliography builder instance for method chaining.</returns>
        BibliographyBuilder Write();
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

        private readonly List<Bibitem> _bibitems = new List<Bibitem>();

        private string _auxPath;
        private IEnumerable<AuxEntry> _auxEntries;
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
            _auxEntries = _auxParser.ParseFile(_auxPath);
            _auxFileParsed = true;

            BibFilePath = _auxEntries.TryGetFirst(x => x.Type == AuxEntryType.Bibdata, out var auxBibdataEntry) ? Path.Combine(texDirectory, $"{auxBibdataEntry.Key}.bib") : null;
            StyleFilePath = _auxEntries.TryGetFirst(x => x.Type == AuxEntryType.Bibstyle, out var auxBibstyleEntry) ? Path.Combine(texDirectory, $"{auxBibstyleEntry.Key}.style.json") : null;

            return this;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">The TexFilePath must be provided prior to running.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The directory containing the TexFilePath, BibFilePath and StyleFilePath must exist.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The files at TexFilePath, BibFilePath and StyleFilePath (if provided) must exist.</exception>
        public BibliographyBuilder Build()
        {
            _logger.LogTrace("Starting build of bibliography.");

            if (!_auxFileParsed)
            { 
                if (TexFilePath == null)
                {
                    throw new ArgumentNullException(nameof(TexFilePath));
                }
                else
                {
                    _fileManager.ThrowIfFileDoesNotExist(TexFilePath);
                }

                if (BibFilePath == null)
                {
                    throw new ArgumentNullException(nameof(BibFilePath));
                }

                if (BibliographyStyle == null && StyleFilePath == null)
                {
                    throw new ArgumentNullException(nameof(BibliographyStyle));
                }

                _auxPath = _fileManager.ReplaceExtension(TexFilePath, "aux");
                _auxEntries = _auxParser.ParseFile(_auxPath);
            }

            _fileManager.ThrowIfFileDoesNotExist(BibFilePath);
            _fileManager.ThrowIfFileDoesNotExist(StyleFilePath);

            var bibtexDatabase = _bibParser.ParseFile(BibFilePath);

            try
            {
                BibliographyStyle = JsonConvert.DeserializeObject<BibliographyStyle>(_fileManager.ReadFileContents(StyleFilePath));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred parsing the style file.");
                throw;
            }

            foreach (var auxEntry in _auxEntries.Where(x => x.Type == AuxEntryType.Bibcite))
            {
                if (bibtexDatabase.Entries.TryGetSingle(bibtexEntry => bibtexEntry.CitationKey == auxEntry.Key, out var bibtexEntry))
                {
                    var style = BibliographyStyle.EntryStyles.SingleOrDefault(s => s.Type == bibtexEntry.EntryType) ?? EntryStyle.Default;
                    _bibitems.Add(new Bibitem(auxEntry, bibtexEntry, style));
                }
            }

            _logger.LogTrace("Bibliography build completed.");

            return this;
        }

        /// <inheritdoc />
        public BibliographyBuilder Write()
        {
            _logger.LogTrace("Starting write of .bbl file.");

            string targetPath = _fileManager.ReplaceExtension(TexFilePath, "bbl");

            _fileManager.DeleteIfExists(targetPath);
            _fileManager.WriteStream(targetPath, writer =>
            {
                writer.WriteLine("\\begin{thebibliography}{1}\r\n");
                foreach (var bibitem in _bibitems)
                {
                    writer.WriteLine($"{bibitem}\r\n");
                }
                writer.WriteLine("\\end{thebibliography}");
            });

            _logger.LogTrace("Finished writing .bbl file.");

            return this;
        }
    }
}