using System;
using System.Collections.Generic;

namespace Bibtex.Abstractions
{
    /// <summary>
    /// Represents a Bibtex Author - the author field is a little more complex than the other fields, so has some special handling.
    /// </summary>
    public class BibtexAuthor
    {
        /// <summary>
        /// The first name of the author.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Any middle names of the author.
        /// </summary>
        public IEnumerable<string> MiddleNames { get; set; }

        /// <summary>
        /// The last name of the author.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The suffix of the author's name, e.g. Jr. or PhD.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Builds a BibtexAuthor from the given string.
        /// This method supports all accepted BibTeX formats: <see href="https://texfaq.org/FAQ-manyauthor"/>.
        /// </summary>
        /// <param name="author">The string to parse into an author</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// /// <exception cref="ArgumentException">Thrown if the provided string is empty or consists solely of whitespace.</exception>
        public static BibtexAuthor FromString(string author)
        {
            if (author is null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            else if (author.Trim().Length == 0)
            {
                throw new ArgumentException($"Parameter should not be empty or whitespace: {nameof(author)}");
            }

            var bibtexAuthor = new BibtexAuthor();
            var commaSplit = author.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (commaSplit.Length == 1) // First Last format.
            {
                var names = commaSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (names.Length > 0)
                {
                    bibtexAuthor.FirstName = names[0];

                    if (names.Length > 1)
                    {
                        bibtexAuthor.LastName = names[^1];

                        if (names.Length > 2)
                        {
                            bibtexAuthor.MiddleNames = names[1..^1];
                        }
                    }
                }
            }
            else if (commaSplit.Length == 2) // Last, First format.
            {
                var firstNames = commaSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var middleNames = new List<string>();
                var lastNames = commaSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (firstNames.Length > 0)
                {
                    bibtexAuthor.FirstName = firstNames[0];

                    if (firstNames.Length > 1)
                    {
                        middleNames.AddRange(firstNames[1..]);
                    }
                }

                if (lastNames.Length > 0)
                {
                    bibtexAuthor.LastName = lastNames[0];

                    if (lastNames.Length > 1)
                    {
                        middleNames.AddRange(lastNames[1..]);
                    }
                }

                bibtexAuthor.MiddleNames = middleNames;
            }
            else if (commaSplit.Length == 3) // Last, Suffix, First format.
            {
                var firstNames = commaSplit[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var suffices = commaSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var lastNames = commaSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var middleNames = new List<string>();

                if (firstNames.Length > 0)
                {
                    bibtexAuthor.FirstName = firstNames[0];

                    if (firstNames.Length > 1)
                    {
                        middleNames.AddRange(firstNames[1..]);
                    }
                }

                if (suffices.Length > 0)
                {
                    bibtexAuthor.Suffix = suffices[0];

                    if (suffices.Length > 1)
                    {
                        middleNames.AddRange(suffices[1..]);
                    }
                }

                if (lastNames.Length > 0)
                {
                    bibtexAuthor.LastName = lastNames[0];

                    if (lastNames.Length > 1)
                    {
                        middleNames.AddRange(lastNames[1..]);
                    }
                }

                bibtexAuthor.MiddleNames = middleNames;
            }
            else
            {
                throw new FormatException($"Unrecognized BibTeX format for author field with value: '{author}'");
            }

            return bibtexAuthor;
        }
    }
}