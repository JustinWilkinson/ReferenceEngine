using Microsoft.Extensions.Logging;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Enumerations;
using ReferenceEngine.Bibtex.Extensions;
using ReferenceEngine.Bibtex.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReferenceEngine.Bibtex.Parser
{
    /// <summary>
    /// Defines the methods used to parse .aux content.
    /// </summary>
    public interface IAuxParser
    {
        /// <summary>
        /// Converts a string into a single AuxEntry instance.
        /// </summary>
        /// <param name="line">The line to parse</param>
        /// <returns>An AuxEntry populated from the line.</returns>
        AuxEntry ParseEntry(string line);

        /// <summary>
        /// Converts an aux file into an enumerable of AuxEntry instances.
        /// </summary>
        /// <param name="path">The path to the file to parse.</param>
        /// <returns>An IEnumerable of parsed AuxEntry instances.</returns>
        IEnumerable<AuxEntry> ParseFile(string path);

        /// <summary>
        /// Converts a stream into an enumerable of AuxEntry instances.
        /// </summary>
        /// <param name="stream">The stream to parse.</param>
        /// <returns>An IEnumerable of parsed AuxEntry instances.</returns>
        IEnumerable<AuxEntry> ParseStream(Stream stream);

        /// <summary>
        /// Converts a multi-line string into an enumerable of AuxEntry instances.
        /// </summary>
        /// <param name="auxContent">The path to the file to parse.</param>
        /// <returns>An IEnumerable of parsed AuxEntry instances.</returns>
        IEnumerable<AuxEntry> ParseString(string auxContent);
    }

    /// <summary>
    /// An implementation of the IAuxParser interface, used to parse .aux content.
    /// </summary>
    public class AuxParser : IAuxParser
    {
        private readonly IFileManager _fileManager;
        private readonly ILogger<AuxParser> _logger;

        /// <summary>
        /// Creates a new AuxParser, initialised with the provided IFileManager and ILogger.
        /// </summary>
        /// <param name="fileManager">The FileManager used to confirm the existence of files.</param>
        /// <param name="logger">The Logger used by this instance.</param>
        public AuxParser(IFileManager fileManager, ILogger<AuxParser> logger)
        {
            _fileManager = fileManager;
            _logger = logger;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IEnumerable<AuxEntry> ParseFile(string path)
        {
            _logger.LogTrace($"Parsing file at '{path}'");

            _fileManager.ThrowIfFileDoesNotExist(path);

            using var fs = File.Open(path, FileMode.Open, FileAccess.Read);
            foreach (var entry in ParseStream(fs))
            {
                yield return entry;
            }

            _logger.LogTrace($"File at '{path}' parsed successfully.");
        }

        /// <inheritdoc />
        public IEnumerable<AuxEntry> ParseStream(Stream stream)
        {
            _logger.LogTrace("Parsing aux stream...");

            string line;
            using var reader = new StreamReader(stream);
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    yield return ParseEntry(line);
                }
            }

            _logger.LogTrace("Aux stream parsed successfully.");
        }

        /// <inheritdoc />
        public IEnumerable<AuxEntry> ParseString(string auxContent)
        {
            _logger.LogTrace("Parsing string containing aux data...");

            using var ms = new MemoryStream(Encoding.ASCII.GetBytes(auxContent));
            foreach (var entry in ParseStream(ms))
            {
                yield return entry;
            }

            _logger.LogTrace("String containing aux data parsed successfully.");
        }
    }
}