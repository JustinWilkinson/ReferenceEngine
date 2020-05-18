using System.Collections.Generic;

namespace LatexReferences.Models.Format
{
    public class BibliographyStyle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<EntryStyle> EntryFormats { get; set; } 
    }
}