using Bibtex.Abstractions;
using Bibtex.Enumerations;
using Bibtex.Extensions;
using Bibtex.Manager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bibtex.Parser
{
    public interface IAuxParser
    {
        AuxEntry ParseEntry(string line);

        IEnumerable<AuxEntry> ParseFile(string path);

        IEnumerable<AuxEntry> ParseStream(Stream stream);

        IEnumerable<AuxEntry> ParseString(string bibtex);
    }

    public class AuxParser : IAuxParser
    {
        private readonly IFileManager _fileManager;
        private readonly ILogger<AuxParser> _logger;

        public AuxParser(IFileManager fileManager, ILogger<AuxParser> logger)
        {
            _fileManager = fileManager;
            _logger = logger;
        }

        public AuxEntry ParseEntry(string line)
        {
            if (line.StartsWith(@"\relax"))
            {
                return new AuxEntry(AuxEntryType.Relax);
            }
            else if (line.StartsWith(@"\bibstyle"))
            {
                return new AuxEntry(AuxEntryType.Bibstyle) { Key = line.Replace(@"\bibstyle", "").TrimIgnoredCharacters() };
            }
            else if (line.StartsWith(@"\citation"))
            {
                return new AuxEntry(AuxEntryType.Citation) { Key = line.Replace(@"\citation", "").TrimIgnoredCharacters() };
            }
            else if (line.StartsWith(@"\bibdata"))
            {
                return new AuxEntry(AuxEntryType.Bibdata) { Key = line.Replace(@"\bibdata", "").TrimIgnoredCharacters() };
            }
            else if (line.StartsWith(@"\bibcite"))
            {
                var entry = new AuxEntry(AuxEntryType.Bibcite);
                var info = line.Replace(@"\bibcite", "").TrimIgnoredCharacters().Split("}{", StringSplitOptions.RemoveEmptyEntries);
                entry.Key = info.Length > 0 ? info[0] : null;
                entry.Label = info.Length > 1 ? info[1] : null;
                return entry;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<AuxEntry> ParseFile(string path)
        {
            _fileManager.ThrowIfFileDoesNotExist(path);

            using var fs = File.Open(path, FileMode.Open, FileAccess.Read);
            foreach (var entry in ParseStream(fs))
            {
                yield return entry;
            }
        }

        public IEnumerable<AuxEntry> ParseStream(Stream stream)
        {
            string line;
            using var reader = new StreamReader(stream);
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    yield return ParseEntry(line);
                }
            }
        }

        public IEnumerable<AuxEntry> ParseString(string auxContent)
        {
            using var ms = new MemoryStream(Encoding.ASCII.GetBytes(auxContent));
            foreach (var entry in ParseStream(ms))
            {
                yield return entry;
            }
        }
    }
}