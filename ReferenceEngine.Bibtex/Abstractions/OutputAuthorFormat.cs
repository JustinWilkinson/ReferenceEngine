using ReferenceEngine.Bibtex.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceEngine.Bibtex.Abstractions
{
    /// <summary>
    /// Represents the format to apply to the Author field.
    /// </summary>
    public class OutputAuthorFormat
    {
        /// <summary>
        /// The number of characters to abbreviate the first name to, if not specified the full name is taken.
        /// Defaults to null.
        /// </summary>
        public int? AbbreviateFirstNameCharacters { get; set; } = null;

        /// <summary>
        /// Specifies whether the last name is listed before the first name.
        /// Defaults to false.
        /// </summary>
        public bool LastNameFirst { get; set; } = false;

        /// <summary>
        /// Specifies whether to include the middle names of authors.
        /// Defaults to false.
        /// </summary>
        public bool IncludeMiddleNames { get; set; } = true;

        /// <summary>
        /// Specifies whether to include any suffices the author may have, e.g. Jr. or PhD.
        /// Defaults to true.
        /// </summary>
        public bool IncludeSuffix { get; set; } = true;

        /// <summary>
        /// The character to used to delimit multiple authors.
        /// Defaults to a comma ','.
        /// </summary>
        public char Delimiter { get; set; } = ',';

        /// <summary>
        /// Allows a special delimiter to be used for the seperate the final two authors.
        /// Defaults to "and".
        /// </summary>
        public string FinalDelimiter { get; set; } = "and";

        /// <summary>
        /// The number of authors to include before truncating, and appending the TruncatedAuthorsText
        /// </summary>
        public int NumberOfNamedAuthors { get; set; } = 3;

        /// <summary>
        /// A LatexString defining whether or not, defaults to "<i>et al.</i>"
        /// </summary>
        public LatexString TruncatedAuthorsText { get; set; } = new LatexString("et al.") { Italic = true };

        /// <summary>
        /// Fully applies this format to the provided author field.
        /// </summary>
        /// <param name="authorField">The contents of the author field.</param>
        /// <returns>A formatted string containing the authors or null if the provided string is null</returns>
        public string FormatAuthorField(string authorField)
        {
            if (authorField != null)
            {
                var authors = GetFormattedIndividualAuthors(authorField).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
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
                    return $"{string.Join($"{Delimiter} ", authors[..^1])} {FinalDelimiter ?? Delimiter.ToString()} {authors[^1]}";
                }
                else
                {
                    return $"{string.Join($"{Delimiter} ", authors[0..NumberOfNamedAuthors])} {TruncatedAuthorsText}";
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Breaks down the contents of the Author field into a collection of BibtexAuthors and formats them.
        /// </summary>
        /// <param name="authorField">The contents of the author field.</param>
        /// <returns>An IEnumerable of formatted author names.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<string> GetFormattedIndividualAuthors(string authorField)
        {
            if (authorField is null)
            {
                throw new ArgumentNullException(nameof(authorField));
            }

            var authors = authorField.Split("and", StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimIgnoredCharacters()).Select(x => BibtexAuthor.FromString(x));

            foreach (var author in authors)
            {
                var authorBuilder = new StringBuilder();
                var firstName = AbbreviateFirstNameCharacters.HasValue && AbbreviateFirstNameCharacters.Value > 0 ? $"{string.Concat(author.FirstName.Take(AbbreviateFirstNameCharacters.Value))}." : author.FirstName;
                var middleNames = IncludeMiddleNames && author.MiddleNames.HasContent() ? $" {string.Join(" ", author.MiddleNames)} " : " ";
                var suffix = IncludeSuffix && author.Suffix != null ? $" {author.Suffix}" : "";

                if (LastNameFirst)
                {
                    authorBuilder.Append($"{author.LastName}, {firstName}{middleNames}{suffix}".TrimEnd());
                }
                else
                {
                    authorBuilder.Append($"{firstName}{middleNames}{author.LastName}{suffix}");
                }

                yield return authorBuilder.ToString().TrimIgnoredCharacters();
            }
        }
    }
}