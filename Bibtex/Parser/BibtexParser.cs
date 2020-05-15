using Bibtex.Abstractions;
using Bibtex.Abstractions.Entries;
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
    public interface IBibtexParser
    {
        BibtexDatabase ParseFile(string path);

        BibtexDatabase ParseString(string databaseName, string bibtex);

        BibtexDatabase ParseStream(string databaseName, Stream stream);

        EntryType? GetEntryType(StreamReader sr);

        BibtexEntry GetEntryContent(EntryType entryType, StreamReader sr);

        Comment GetComment(StreamReader sr);

        Preamble GetPreamble(StreamReader sr);

        StringEntry GetStringEntry(StreamReader sr);
    }

    public class BibtexParser : IBibtexParser
    {
        private readonly IFileManager _fileManager;
        private readonly ILogger<BibtexParser> _logger;

        public BibtexParser(IFileManager fileManager, ILogger<BibtexParser> logger)
        {
            _fileManager = fileManager;
            _logger = logger;
        }

        public BibtexDatabase ParseFile(string path)
        {
            _fileManager.ThrowIfFileDoesNotExist(path);
            using var fs = File.Open(path, FileMode.Open, FileAccess.Read);
            return ParseStream(Path.GetFileName(path), fs);
        }

        public BibtexDatabase ParseString(string databaseName, string bibtex)
        {
            using var ms = new MemoryStream(Encoding.ASCII.GetBytes(bibtex));
            return ParseStream(databaseName, ms);
        }

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
                        keyValuePairs.Add(split[0].TrimIgnoredCharacters().ReplaceBraces(), split[1].TrimIgnoredCharacters().ReplaceBraces());
                    }
                }

                return new BibtexEntry(entryType, citationKey, keyValuePairs);
            }
            else
            {
                return null;
            }
        }

        public Comment GetComment(StreamReader sr) => new Comment(GetTextBetweenBraces(sr));

        public Preamble GetPreamble(StreamReader sr) => new Preamble(GetTextBetweenBraces(sr));

        public StringEntry GetStringEntry(StreamReader sr) => new StringEntry(GetTextBetweenBraces(sr));

        #region Private
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
                    }
                    leftBraceCount++;
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
                    lst.Add(current.ToString().Trim(' ', '"', '{', '}', '\t', '\r', '\n'));
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