using System.Collections.Generic;

namespace LatexReferences.Models
{
    public class BibliographyStyle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<EntryStyle> EntryStyles { get; set; } 
    }
}