using Bibtex.Abstractions.Entries;
using System.Collections.Generic;

namespace Bibtex.Abstractions
{
    /// <summary>
    /// Represents a .bib file.
    /// </summary>
    public class BibtexDatabase
    {
        /// <summary>
        /// The name of the database (typically the name of the .bib file).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The String entries found in the database.
        /// </summary>
        public List<StringEntry> Strings { get; set; } = new List<StringEntry>();

        /// <summary>
        /// The Preamble entries found in the database.
        /// </summary>
        public List<Preamble> Preambles { get; set; } = new List<Preamble>();

        /// <summary>
        /// The Comments found in the database.
        /// </summary>
        public List<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// The citable Bibtex Entries found in the database.
        /// </summary>
        public List<BibtexEntry> Entries { get; set; } = new List<BibtexEntry>();

        /// <summary>
        /// A computed count of all the types of entries.
        /// </summary>
        public int AllEntriesCount => Strings.Count + Preambles.Count + Comments.Count + Entries.Count;

        /// <summary>
        /// Constructs a new BibtexDatabase and assigns it the provided name.
        /// </summary>
        /// <param name="name">The name of the database.</param>
        public BibtexDatabase(string name)
        {
            Name = name;
        }
    }
}