using System.Collections.Generic;

namespace LatexReferences.Models.Format
{
    public class FullFormat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<BibEntryFormat> EntryFormats { get; set; } 
    }
}