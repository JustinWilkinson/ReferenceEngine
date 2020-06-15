using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Enumerations;
using ReferenceEngine.Bibtex.Extensions;
using ReferenceEngine.Bibtex.Manager;
using ReferenceEngine.Bibtex.Parser;
using System;
using System.Collections.Generic;
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
        public string TexFilePath { get; set; }

        /// <summary>
        /// The path to the .bib file to read.
        /// </summary>
        public string BibFilePath { get; set; }

        /// <summary>
        /// The path to the style file to read.
        /// </summary>
        public string StyleFilePath { get; set; }

        /// <summary>
        /// The bibliography style to use.
        /// </summary>
        public BibliographyStyle BibliographyStyle { get; set; }

        /// <summary>
        /// Builds the BibliographyStyle and applies it to the contents of the BibFilePath.
        /// </summary>
        public void Build();

        /// <summary>
        /// Writes the bibliography to a .bbl file.
        /// </summary>
        public void Write();
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
        /// <exception cref="ArgumentNullException">The TexFilePath, BibFilePath and BibliographyStyle (or a path to the Style file) must be provided</exception>
        /// <exception cref="DirectoryNotFoundException">The directory containing the TexFilePath, BibFilePath and StyleFilePath (if provided) must exist</exception>
        /// <exception cref="FileNotFoundException">The TexFilePath, BibFilePath and StyleFilePath (if provided) must exist</exception>
        public void Build()
        {
            _logger.LogTrace("Starting build of bibliography.");

            if (TexFilePath == null)
            {
                throw new ArgumentNullException(nameof(TexFilePath));
            }
            else if (BibFilePath == null)
            {
                throw new ArgumentNullException(nameof(BibFilePath));
            }
            else if (BibliographyStyle == null)
            {
                if (StyleFilePath != null)
                {
                    _fileManager.ThrowIfFileDoesNotExist(StyleFilePath);
                    try
                    {
                        BibliographyStyle = JsonConvert.DeserializeObject<BibliographyStyle>(_fileManager.ReadFileContents(StyleFilePath));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred parsing the style file.");
                        throw;
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(BibliographyStyle));
                }
            }

            _fileManager.ThrowIfFileDoesNotExist(TexFilePath);
            _fileManager.ThrowIfFileDoesNotExist(BibFilePath);
            var auxPath = _fileManager.ReplaceExtension(TexFilePath, "aux");

            var auxEntries = _auxParser.ParseFile(auxPath);
            var bibtexDatabase = _bibParser.ParseFile(BibFilePath);

            foreach (var auxEntry in auxEntries.Where(x => x.Type == AuxEntryType.Bibcite))
            {
                if (bibtexDatabase.Entries.TryGetSingle(bibtexEntry => bibtexEntry.CitationKey == auxEntry.Key, out var bibtexEntry))
                {
                    var style = BibliographyStyle.EntryStyles.SingleOrDefault(s => s.Type == bibtexEntry.EntryType) ?? EntryStyle.Default;
                    _bibitems.Add(new Bibitem(auxEntry, bibtexEntry, style));
                }
            }

            _logger.LogTrace("Bibliography build completed.");
        }

        /// <inheritdoc />
        public void Write()
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
        }
    }
}