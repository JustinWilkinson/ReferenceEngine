using Bibtex.Abstractions;
using Bibtex.Enumerations;
using Bibtex.Manager;
using Bibtex.Parser;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bibtex
{
    public interface IBibliographyBuilder
    {
        public string TexFilePath { get; set; }

        public string BibFilePath { get; set; }

        public void Build();

        public void Write();
    }

    public class BibliographyBuilder : IBibliographyBuilder
    {
        private readonly IFileManager _fileManager;
        private readonly IAuxParser _auxParser;
        private readonly IBibtexParser _bibParser;
        private readonly ILogger<BibliographyBuilder> _logger;

        private readonly List<Bibitem> _bibitems = new List<Bibitem>();

        public BibliographyBuilder(IFileManager fileManager, IAuxParser auxParser, IBibtexParser bibParser, ILogger<BibliographyBuilder> logger)
        {
            _fileManager = fileManager;
            _auxParser = auxParser;
            _bibParser = bibParser;
            _logger = logger;
            CustomItemTemplates = new Dictionary<EntryType, BibitemTemplate>();
        }

        public string TexFilePath { get; set; }

        public string BibFilePath { get; set; }

        public Dictionary<EntryType, BibitemTemplate> CustomItemTemplates { get; set; }

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

            _fileManager.ThrowIfFileDoesNotExist(TexFilePath);
            _fileManager.ThrowIfFileDoesNotExist(BibFilePath);
            var auxPath = _fileManager.ReplaceExtension(TexFilePath, "aux");

            var auxEntries = _auxParser.ParseFile(auxPath);
            var bibtexDatabase = _bibParser.ParseFile(BibFilePath);

            foreach (var auxEntry in auxEntries.Where(x => x.Type == AuxEntryType.Bibcite))
            {
                var bibtexEntry = bibtexDatabase.Entries.SingleOrDefault(bibtexEntry => bibtexEntry.CitationKey == auxEntry.Key);
                if (bibtexEntry != null)
                {
                    var bibitemTemplate = CustomItemTemplates.TryGetValue(bibtexEntry.EntryType, out var template) ? template : BibitemTemplate.GetDefaultTemplate(bibtexEntry.EntryType);
                    _bibitems.Add(new Bibitem(auxEntry, bibtexEntry, bibitemTemplate));
                }
            }

            _logger.LogTrace("Bibliography build completed.");
        }

        public void Write()
        {
            _logger.LogTrace("Starting write of .bbl file.");

            string targetPath = _fileManager.ReplaceExtension(TexFilePath, "bbl");

            File.Delete(targetPath);

            using var writer = new StreamWriter(targetPath);
            writer.WriteLine("\\begin{thebibliography}{1}\r\n");
            foreach (var bibitem in _bibitems)
            {
                writer.WriteLine($"{bibitem}\r\n");
            }
            writer.WriteLine("\\end{thebibliography}");

            _logger.LogTrace("Starting build of .bbl file.");
        }
    }
}