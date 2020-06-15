using System.Collections.Generic;

namespace ReferenceEngine.Bibtex.Abstractions
{
    /// <summary>
    /// Represents a style to apply to a bibliography.
    /// </summary>
    public class BibliographyStyle
    {
        /// <summary>
        /// The Id of the Bibliography Style, for easy identification.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Name of the Bibliography Style, for easy identification.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The styles to apply to entries in the database.
        /// </summary>
        public List<EntryStyle> EntryStyles { get; set; } = new List<EntryStyle>();
    }
}