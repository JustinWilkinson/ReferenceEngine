using Microsoft.Extensions.Logging;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Abstractions.Entries;
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
    /// Defines the methods used to parse Bibtex content.
    /// </summary>
    public interface IBibtexParser
    {
        /// <summary>
        /// Parses contents of a file into a BibtexDatabase.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns>A BibtexDatabase populated with the entries parsed from the file.</returns>
        BibtexDatabase ParseFile(string path);

        /// <summary>
        /// Parses contents of a string into a BibtexDatabase.
        /// </summary>
        /// <param name="databaseName">The name to provide to the BibtexDatabase.</param>
        /// <param name="bibtex">Bibtex content to parse.</param>
        /// <returns>A BibtexDatabase populated with the entries parsed from the string.</returns>
        BibtexDatabase ParseString(string databaseName, string bibtex);

        /// <summary>
        /// Parses contents of a Stream into a BibtexDatabase.
        /// </summary>
        /// <param name="databaseName">The name to provide to the BibtexDatabase.</param>
        /// <param name="stream">Stream containing Bibtex content to parse.</param>
        /// <returns>A BibtexDatabase populated with the entries parsed from the stream.</returns>
        BibtexDatabase ParseStream(string databaseName, Stream stream);

        /// <summary>
        /// Extracts the entry type of the next BibtexEntry from a StreamReader.
        /// </summary>
        /// <param name="sr">StreamReader containing Bibtex content.</param>
        /// <returns>The type of the next entry, or null if unrecognized.</returns>
        EntryType? GetEntryType(StreamReader sr);

        /// <summary>
        /// Extracts the contents of a BibtexEntry from a StreamReader.
        /// </summary>
        /// <param name="entryType">Stream containing Bibtex content.</param>
        /// <param name="sr">StreamReader containing Bibtex content.</param>
        /// <returns>A BibtexEntry extracted from the StreamReader.</returns>
        BibtexEntry GetEntryContent(EntryType entryType, StreamReader sr);

        /// <summary>
        /// Extracts the contents of a Comment entry from a StreamReader.
        /// </summary>
        /// <param name="sr">StreamReader containing Bibtex content.</param>
        /// <returns>A Comment extracted from the StreamReader.</returns>
        Comment GetComment(StreamReader sr);

        /// <summary>
        /// Extracts the contents of a Preamble entry from a StreamReader.
        /// </summary>
        /// <param name="sr">StreamReader containing Bibtex content.</param>
        /// <returns>A Preamble extracted from the StreamReader.</returns>
        Preamble GetPreamble(StreamReader sr);

        /// <summary>
        /// Extracts the contents of a StringEntry from a StreamReader.
        /// </summary>
        /// <param name="sr">StreamReader containing Bibtex content.</param>
        /// <returns>A StringEntry extracted from the StreamReader.</returns>
        StringEntry GetStringEntry(StreamReader sr);
    }

    /// <summary>
    /// An implementation of the IBibtexParser interface, used to parse Bibtex content.
    /// </summary>
    public class BibtexParser : IBibtexParser
    {
        private readonly IFileManager _fileManager;
        private readonly ILogger<BibtexParser> _logger;

        /// <summary>
        /// Creates a new BibtexParser, initialised with the provided IFileManager and ILogger.
        /// </summary>
        /// <param name="fileManager">The FileManager used to confirm the existence of files.</param>
        /// <param name="logger">The Logger used by this instance.</param>
        public BibtexParser(IFileManager fileManager, ILogger<BibtexParser> logger)
        {
            _fileManager = fileManager;
            _logger = logger;
        }

        /// <inheritdoc />
        public BibtexDatabase ParseFile(string path)
        {
            _fileManager.ThrowIfFileDoesNotExist(path);
            using var fs = File.Open(path, FileMode.Open, FileAccess.Read);
            return ParseStream(Path.GetFileName(path), fs);
        }

        /// <inheritdoc />
        public BibtexDatabase ParseString(string databaseName, string bibtex)
        {
            using var ms = new MemoryStream(Encoding.ASCII.GetBytes(bibtex));
            return ParseStream(databaseName, ms);
        }

        /// <inheritdoc />
        public BibtexDatabase ParseStream(string databaseName, Stream stream)
        {
            var database = new BibtexDatabase(databaseName);

            int failedTags = 0;
            using var sr = new StreamReader(stream);
            while (sr.Peek() >= 0)
            {
                try
                {
                    var entryType = GetEntryType(sr);

                    if (entryType.HasValue)
                    {
                        switch (entryType)
                        {
                            case EntryType.Comment:
                                database.Comments.Add(GetComment(sr));
                                break;
                            case EntryType.Preamble:
                                database.Preambles.Add(GetPreamble(sr));
                                break;
                            case EntryType.String:
                                database.Strings.Add(GetStringEntry(sr));
                                break;
                            default:
                                database.Entries.Add(GetEntryContent(entryType.Value, sr));
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"An error occurred parsing the Bibtex stream at entry: {database.AllEntriesCount + ++failedTags} - this tag will be skipped.");
                }
            }

            return database;
        }

        /// <inheritdoc />
        public EntryType? GetEntryType(StreamReader sr)
        {
            StringBuilder tagBuilder = new StringBuilder();
            bool foundStart = false;
            int charCode;
            while ((charCode = sr.Peek()) >= 0)
            {
                var character = Convert.ToChar(charCode);

                if (foundStart)
                {
                    if (character == '{' || character == '(' || character == ' ')
                    {
                        break;
                    }
                    else
                    {
                        tagBuilder.Append(Convert.ToChar(sr.Read()));
                    }
                }
                else
                {
                    if (character == '@')
                    {
                        foundStart = true;
                    }

                    sr.Read();
                }
            }
            return Enum.TryParse(typeof(EntryType), tagBuilder.ToString(), true, out var result) ? (EntryType?)result : null;
        }

        /// <inheritdoc />
        public BibtexEntry GetEntryContent(EntryType entryType, StreamReader sr)
        {
            var keyValuePairs = new Dictionary<string, string>();
            List<string> fields = SplitOnUnquotedCharacter(GetTextBetweenBraces(sr));

            if (fields.Count > 0)
            {
                var citationKey = fields[0];
                fields.RemoveAt(0);

                foreach (var field in fields)
                {
                    var split = field.TrimIgnoredCharacters().Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length == 2)
                    {
                        keyValuePairs.Add(split[0].TrimIgnoredCharacters().RemoveBraces(), split[1].TrimIgnoredCharacters().RemoveBraces());
                    }
                }

                return new BibtexEntry(entryType, citationKey, keyValuePairs);
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc />
        public Comment GetComment(StreamReader sr) => new Comment(GetTextBetweenBraces(sr));

        /// <inheritdoc />
        public Preamble GetPreamble(StreamReader sr) => new Preamble(GetTextBetweenBraces(sr));

        /// <inheritdoc />
        public StringEntry GetStringEntry(StreamReader sr) => new StringEntry(GetTextBetweenBraces(sr));

        #region Private
        /// <summary>
        /// Extract the contents of a StreamReader contained in a set of braces.
        /// </summary>
        /// <param name="sr">StreamReader to read</param>
        /// <returns>A string contained between a set of braces.</returns>
        private string GetTextBetweenBraces(StreamReader sr)
        {
            var content = new StringBuilder();

            var insideBraces = false;
            var leftBraceCount = 0;
            var rightBraceCount = 0;
            char? openingBraceCharacter = null;
            char? closingBraceCharacter = null;
            while (sr.Peek() >= 0)
            {
                var character = Convert.ToChar(sr.Read());

                if (character == '{' || character == '(')
                {
                    if (!openingBraceCharacter.HasValue)
                    {
                        openingBraceCharacter = character;
                        closingBraceCharacter = openingBraceCharacter == '{' ? '}' : ')';
                        leftBraceCount++;
                    } 
                    else if (character == openingBraceCharacter.Value)
                    {
                        leftBraceCount++;
                    }
                }
                else if (closingBraceCharacter.HasValue && character == closingBraceCharacter.Value)
                {
                    rightBraceCount++;
                }

                if (leftBraceCount > 0)
                {
                    if (leftBraceCount == rightBraceCount)
                    {
                        break;
                    }
                    else if (insideBraces)
                    {
                        content.Append(character);
                    }
                    else
                    {
                        insideBraces = true;
                    }
                }
                else if (char.IsLetterOrDigit(character))
                {
                    throw new FormatException($"Unexpected character: '{character}' between tag start and first brace!");
                }
            }

            return content.ToString();
        }

        /// <summary>
        /// Split a string on a given character, provided it is not contained in quotes.
        /// </summary>
        /// <param name="input">String to split.</param>
        /// <param name="splitChar">Character to split on, defaults to a comma ','.</param>
        /// <returns>A list of strings </returns>
        private List<string> SplitOnUnquotedCharacter(string input, char splitChar = ',')
        {
            List<string> lst = new List<string>();

            var leftBraceCount = 0;
            var rightBraceCount = 0;
            var inQuotes = false;

            StringBuilder current = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == '"')
                {
                    inQuotes = leftBraceCount == rightBraceCount ? !inQuotes : inQuotes;
                }
                else if (!inQuotes && leftBraceCount == rightBraceCount && c == splitChar)
                {
                    lst.Add(current.ToString().TrimIgnoredCharacters());
                    current.Clear();
                }
                else
                {
                    if (c == '{')
                    {
                        leftBraceCount++;
                    }
                    else if (c == '}')
                    {
                        rightBraceCount++;
                    }

                    current.Append(c);
                }
            }

            lst.Add(current.ToString());

            return lst;
        }
        #endregion
    }
}