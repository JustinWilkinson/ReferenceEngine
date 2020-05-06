using Bibtex.Abstractions;
using Bibtex.Enumerations;
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
        private readonly ILogger<BibliographyBuilder> _logger;
        private readonly IAuxParser _auxParser;
        private readonly IBibtexParser _bibParser;

        private readonly List<Bibitem> _bibitems = new List<Bibitem>();

        public BibliographyBuilder(IAuxParser auxParser, IBibtexParser bibParser, ILogger<BibliographyBuilder> logger)
        {
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
                throw new ArgumentNullException("TexFilePath to build bibliography for cannot be null!");
            }
            else if (BibFilePath == null)
            {
                throw new ArgumentNullException("BibFilePath cannot be null!");
            }

            var texDirectory = Path.GetDirectoryName(TexFilePath);
            var texFileNameWithoutExtension = Path.GetFileNameWithoutExtension(TexFilePath);
            var bibDirectory = Path.GetDirectoryName(BibFilePath);

            if (!Directory.Exists(texDirectory))
            {
                throw new DirectoryNotFoundException($"Could not find directory: '{texDirectory}'");
            }
            else if (!Directory.Exists(bibDirectory))
            {
                throw new DirectoryNotFoundException($"Could not find directory: '{bibDirectory}'");
            }
            else if (!File.Exists(TexFilePath))
            {
                throw new FileNotFoundException($"Could not find file: '{Path.GetFileName(TexFilePath)}' in '{texDirectory}'");
            }
            else if (!File.Exists(BibFilePath))
            {
                throw new FileNotFoundException($"Could not find file: '{Path.GetFileName(BibFilePath)}' in '{bibDirectory}'");
            }

            var auxEntries = _auxParser.ParseFile(Path.Combine(texDirectory, $"{texFileNameWithoutExtension}.aux"));
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

            string targetPath = Path.Combine(Path.GetDirectoryName(TexFilePath), $"{Path.GetFileNameWithoutExtension(TexFilePath)}.bbl");

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