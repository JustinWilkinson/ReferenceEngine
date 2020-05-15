using Bibtex.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bibtex.Abstractions
{
    public class OutputAuthorFormat
    {
        public bool RespectBibtexAbbreviation { get; set; }

        public int? AbbreviateFirstNameCharacters { get; set; }

        public bool LastNameFirst { get; set; }

        public bool IncludeMiddleNames { get; set; }

        public bool IncludeSuffix { get; set; }

        public char Delimiter { get; set; }

        public string FinalDelimiter { get; set; }

        public int NumberOfNamedAuthors { get; set; }

        public LatexString TruncatedAuthors { get; set; }

        public static OutputAuthorFormat Default { get; } = new OutputAuthorFormat
        {
            RespectBibtexAbbreviation = true,
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
                    return $"{string.Join($"{Delimiter} ", authors[..^1])} {FinalDelimiter} {authors[^1]}";
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
            var authors = authorField.Split("and", StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimIgnoredCharacters()).Select(x => BibtexAuthor.FromString(x));

            foreach (var author in authors)
            {
                var authorBuilder = new StringBuilder();
                var firstName = AbbreviateFirstNameCharacters.HasValue ? $"{author.FirstName.Take(AbbreviateFirstNameCharacters.Value)}." : author.FirstName;
                var middleNames = IncludeMiddleNames ? $" {string.Join(" ", author.MiddleNames)}" : "";
                var suffix = IncludeSuffix ? $" {author.Suffix}" : "";

                if (LastNameFirst)
                {
                    authorBuilder.Append($"{author.LastName},{firstName}{middleNames}{suffix}");
                }
                else
                {
                    authorBuilder.Append($"{firstName}{middleNames}{author.LastName}{suffix}");
                }

                yield return authorBuilder.ToString();
            }
        }
    }
}