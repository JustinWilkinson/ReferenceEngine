using Bibtex.Abstractions.Entries;
using System.Collections.Generic;

namespace Bibtex.Abstractions
{
    public class BibtexDatabase
    {
        public string Name { get; set; }

        public List<StringEntry> Strings { get; set; } = new List<StringEntry>();

        public List<Preamble> Preambles { get; set; } = new List<Preamble>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<BibtexEntry> Entries { get; set; } = new List<BibtexEntry>();

        public int AllEntriesCount => Strings.Count + Preambles.Count + Comments.Count + Entries.Count;

        public BibtexDatabase(string name)
        {
            Name = name;
        }
    }
}