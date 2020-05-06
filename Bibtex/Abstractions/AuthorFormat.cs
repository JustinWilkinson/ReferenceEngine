using Bibtex.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bibtex.Abstractions
{
    public class AuthorFormat
    {
        public bool RespectBibtexAbbreviation { get; set; }

        public int? AbbreviateFirstNameCharacters { get; set; }

        public bool LastNameFirst { get; set; }

        public char Delimiter { get; set; }

        public string FinalDelimiter { get; set; }

        public int NumberOfNamedAuthors { get; set; }

        public LatexString TruncatedAuthors { get; set; }

        public static AuthorFormat Default { get; } = new AuthorFormat 
        { 
            RespectBibtexAbbreviation = true,
            LastNameFirst = false,
            Delimiter = ',',
            FinalDelimiter = "and",
            NumberOfNamedAuthors = 3,
            TruncatedAuthors = new LatexString("et al") { Italic = true } 
        };

        public string FormatAuthorField(string authorField)
        {
            if (authorField != null)
            {
                var authors = GetFormattedAuthors(authorField).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                if (authors.Length == 0)
                {
                    return null;
                }
                else if (authors.Length == 1)
                {
                    return authors[0];
                }
                else if (authors.Length <= NumberOfNamedAuthors)
                {
                    return $"{string.Join($"{Delimiter} ", authors[0..^2])} {FinalDelimiter} {authors[^1]}";
                }
                else
                {
                    return $"{string.Join($"{Delimiter} ", authors[0..NumberOfNamedAuthors])} {TruncatedAuthors}";
                }
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<string> GetFormattedAuthors(string authorField)
        {
            var authors = authorField.Split(new[] { ",", "and" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimIgnoredCharacters());

            foreach (var author in authors)
            {
                if (LastNameFirst || AbbreviateFirstNameCharacters.HasValue)
                {
                    var names = author.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (names.Length == 0)
                    {
                        continue;
                    }
                    else if (names.Length == 1)
                    {
                        yield return names[0];
                    }
                    else
                    {
                        if (AbbreviateFirstNameCharacters.HasValue)
                        {
                            names[0] = names[0].Take(AbbreviateFirstNameCharacters.Value).ToString();
                        }

                        if (LastNameFirst)
                        {
                            yield return $"{names[^0]} {string.Join(" ", names[0..^1])}";
                        }
                        else
                        {
                            yield return $"{string.Join(" ", names)}";
                        }
                    }
                }
                else
                {
                    yield return author;
                }
            }
        }
    }
}