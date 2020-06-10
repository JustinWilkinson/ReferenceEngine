using System.Collections.Generic;

namespace Bibtex.Abstractions
{
    public class BibliographyStyle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<EntryStyle> EntryStyles { get; set; } = new List<EntryStyle>();
    }
}