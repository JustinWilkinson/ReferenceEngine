using System;
using System.Collections.Generic;

namespace Bibtex.Abstractions
{
    public class BibtexAuthor
    {
        public string FirstName { get; set; }

        public IEnumerable<string> MiddleNames { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public static BibtexAuthor FromString(string author)
        {
            if (author is null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            else if (author.Trim().Length == 0)
            {
                throw new FormatException($"Parameter should not be empty or whitespace: {nameof(author)}");
            }

            // Parse author field using accepted BibTeX formats: https://texfaq.org/FAQ-manyauthor.
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