using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Abstractions.Entries;
using ReferenceEngine.Bibtex.Enumerations;
using ReferenceEngine.Bibtex.Extensions;
using ReferenceEngine.Bibtex.Manager;
using ReferenceEngine.Bibtex.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReferenceEngine.Bibtex
{
    /// <summary>
    /// Defines the methods required to build a bibliography style and write a styled .bbl file.
    /// </summary>
    public interface IBibliographyBuilder
    {
        /// <summary>
        /// The path to the .tex file to read. This must be set prior to calling <see cref="Build"/>.
        /// </summary>
        string TexFilePath { get; set; }

        /// <summary>
        /// The path to the .bib file to read. This must be set prior to calling <see cref="Build"/>.
        /// </summary>
        string BibFilePath { get; set; }

        /// <summary>
        /// The path to the style file to read. This, or <see cref="BibliographyStyle"/> must be set prior to calling <see cref="Build"/>.
        /// </summary>
        string StyleFilePath { get; set; }

        /// <summary>
        /// The bibliography style to use. This, or <see cref="StyleFilePath"/> must be set prior to calling <see cref="Build"/>.
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
        private readonly ILogger<Bibliography> _bibliographyLogger;

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
            _bibliographyLogger = new LoggerFactory().CreateLogger<Bibliography>();
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
                else if (BibFilePath == null)
                {
                    throw new InvalidOperationException("BibFilePath cannot be null!");
                }
                else if (BibliographyStyle == null && StyleFilePath == null)
                {
                    throw new InvalidOperationException("Either the BibliographyStyle, or the StyleFilePath cannot be null!");
                }

                _fileManager.ThrowIfFileDoesNotExist(BibFilePath);
                _fileManager.ThrowIfFileDoesNotExist(TexFilePath);

                if (StyleFilePath != null)
                {
                    _fileManager.ThrowIfFileDoesNotExist(StyleFilePath);
                }

                _auxPath = _fileManager.ReplaceExtension(TexFilePath, "aux");
                _auxEntries = _auxParser.ParseFile(_auxPath).ToList();
            }

            if (BibliographyStyle == null && StyleFilePath != null)
            {
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

            var bibtexDatabase = _bibParser.ParseFile(BibFilePath);
            var bibliography = new Bibliography(_fileManager, _bibliographyLogger, BibliographyStyle.OrderBy)
            {
                TargetPath = _fileManager.ReplaceExtension(TexFilePath, "bbl"),
                TargetAuxPath = _auxPath,
                Preambles = bibtexDatabase.Preambles.Select(x => x.Content).ToList(),
                Bibitems = BibliographyStyle.OrderBy switch
                {
                    BibliographyOrder.AuthorLastName => GetBibitemsOrderedByFirstAuthorLastName(bibtexDatabase),
                    BibliographyOrder.Title => GetBibitemsOrderedByTitle(bibtexDatabase),
                    _ => GetBibitemsInOrderOfAppearance(bibtexDatabase).ToList()
                }
            };

            PerformStringSubstitutions(bibliography, bibtexDatabase);

            _logger.LogTrace("Bibliography build completed.");

            return bibliography;
        }

        #region Private
        private IEnumerable<Bibitem> GetBibitemsInOrderOfAppearance(BibtexDatabase bibtexDatabase)
        {
            // Build up unique collection of bibitems.
            var citationKeys = new HashSet<string>();
            foreach (var auxEntry in _auxEntries.Where(x => x.Type == AuxEntryType.Citation))
            {
                if (!citationKeys.Contains(auxEntry.Key))
                {
                    citationKeys.Add(auxEntry.Key);

                    if (bibtexDatabase.Entries.TryGetFirst(bibtexEntry => bibtexEntry.CitationKey == auxEntry.Key, out var bibtexEntry))
                    {
                        var style = BibliographyStyle.EntryStyles.FirstOrDefault(s => s.Type == bibtexEntry.EntryType) ?? EntryStyle.Default;
                        yield return new Bibitem(citationKeys.Count, auxEntry, bibtexEntry, style);
                    }
                    else
                    {
                        var warning = $"No bibliography entry matching citation key: '{auxEntry.Key}' found.";
                        _logger.LogWarning(warning);
                        yield return new Bibitem(citationKeys.Count, auxEntry.Key, auxEntry.Label, warning);
                    }
                }
            }
        }

        private List<Bibitem> GetBibitemsOrderedByFirstAuthorLastName(BibtexDatabase bibtexDatabase) => GetOrderedItems(GetMatchedEntries(bibtexDatabase), x => BibtexAuthor.GetFirstAuthorLastName(x.BibtexEntry?.Author) ?? x.BibtexEntry?.Title).ToList();

        private List<Bibitem> GetBibitemsOrderedByTitle(BibtexDatabase bibtexDatabase) => GetOrderedItems(GetMatchedEntries(bibtexDatabase), x => x.BibtexEntry?.Title).ToList();

        private List<(AuxEntry AuxEntry, BibtexEntry BibtexEntry, EntryStyle EntryStyle)> GetMatchedEntries(BibtexDatabase bibtexDatabase)
        {
            // Build up unique collection of bibtex entries.
            var matchedEntries = new List<(AuxEntry AuxEntry, BibtexEntry BibtexEntry, EntryStyle EntryStyle)>();
            var citationKeys = new HashSet<string>();

            foreach (var auxEntry in _auxEntries.Where(x => x.Type == AuxEntryType.Citation))
            {
                if (!citationKeys.Contains(auxEntry.Key))
                {
                    citationKeys.Add(auxEntry.Key);
                    var bibtexEntry = bibtexDatabase.Entries.FirstOrDefault(bibtexEntry => bibtexEntry.CitationKey == auxEntry.Key);
                    var entryStyle = bibtexEntry != null ? (BibliographyStyle.EntryStyles.FirstOrDefault(s => s.Type == bibtexEntry.EntryType) ?? EntryStyle.Default) : null;
                    matchedEntries.Add((auxEntry, bibtexEntry, entryStyle));
                }
            }

            return matchedEntries;
        }

        private IEnumerable<Bibitem> GetOrderedItems<T>(List<(AuxEntry AuxEntry, BibtexEntry BibtexEntry, EntryStyle EntryStyle)> matchedEntries, Func<(AuxEntry AuxEntry, BibtexEntry BibtexEntry, EntryStyle EntryStyle), T> keySelector)
        {
            var index = 0;
            foreach (var matchedEntry in matchedEntries.OrderBy(x => keySelector(x)))
            {
                if (matchedEntry.BibtexEntry != null)
                {
                    yield return new Bibitem(++index, matchedEntry.AuxEntry, matchedEntry.BibtexEntry, matchedEntry.EntryStyle);
                }
                else
                {
                    var warning = $"No bibliography entry matching citation key: '{matchedEntry.AuxEntry.Key}' found.";
                    _logger.LogWarning(warning);
                    yield return new Bibitem(++index, matchedEntry.AuxEntry.Key, matchedEntry.AuxEntry.Label, warning);
                }
            }
        }

        private void PerformStringSubstitutions(Bibliography bibliography, BibtexDatabase bibtexDatabase)
        {
            for (var i = 0; i < bibtexDatabase.Strings.Count; i++)
            {
                var stringContent = bibtexDatabase.Strings[i].Content;
                if (i == bibtexDatabase.Strings.Count - 1)
                {
                    bibliography.Preambles = bibtexDatabase.Preambles.Select(p => SubstituteAndConcatenate(p.Content, stringContent)).ToList();
                    bibliography.Bibitems.ForEach(bibitem => bibitem.Detail = SubstituteAndConcatenate(bibitem.Detail, stringContent));
                }
                else
                {
                    bibliography.Preambles = bibtexDatabase.Preambles.Select(preamble => preamble.Content.Substitute(stringContent, '#')).ToList();
                    bibliography.Bibitems.ForEach(bibitem => bibitem.Detail = bibitem.Detail.Substitute(stringContent, '#'));
                }
            }
        }

        private string SubstituteAndConcatenate(string str, KeyValuePair<string, string> kvp) => str.Substitute(kvp, '#', " ", x => x.Trim().RemoveFromStart('{', '"').RemoveFromEnd('}', '"'));
        #endregion
    }
}