using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReferenceEngine.Bibtex.Enumerations;
using System.Collections.Generic;
using System.ComponentModel;

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
        /// Controls the bibliography entry ordering.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [DisplayName("Order By")]
        public BibliographyOrder OrderBy { get; set; } = BibliographyOrder.Appearance;

        /// <summary>
        /// The styles to apply to entries in the database.
        /// </summary>
        public List<EntryStyle> EntryStyles { get; set; } = new List<EntryStyle>();
    }
}