using ReferenceEngine.Bibtex.Abstractions.Entries;
using System;

namespace ReferenceEngine.Bibtex.Abstractions
{
    /// <summary>
    /// Represents a Bibitem in the .bbl file.
    /// </summary>
    public class Bibitem
    {
        /// <summary>
        /// Constructs a new Bibitem and assigns string values to a bibitem.
        /// </summary>
        public Bibitem(string citationKey = null, string label = null, string detail = null)
        {
            CitationKey = citationKey;
            Label = label;
            Detail = detail;
        }

        /// <summary>
        /// Constructs and initialises a styled Bibitem.
        /// </summary>
        /// <param name="auxEntry">The aux entry associated with the citation.</param>
        /// <param name="bibtexEntry">The bibtex entry associated to the citation key.</param>
        /// <param name="entryStyle">The style to apply to the Bibitem.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// /// <exception cref="ArgumentException">Thrown when the AuxEntry key and the BibtexEntry key do not match.</exception>
        public Bibitem(AuxEntry auxEntry, BibtexEntry bibtexEntry, EntryStyle entryStyle)
        {
            if (auxEntry is null)
            {
                throw new ArgumentNullException(nameof(auxEntry));
            }
            if (bibtexEntry is null)
            {
                throw new ArgumentNullException(nameof(bibtexEntry));
            }
            if (entryStyle is null)
            {
                throw new ArgumentNullException(nameof(entryStyle));
            }
            if (!auxEntry.Key.Equals(bibtexEntry.CitationKey, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The AuxEntry Key '{auxEntry.Key}' must match the BibtexEntry CitationKey '{bibtexEntry.CitationKey}'!");
            }

            CitationKey = auxEntry.Key;
            Label = auxEntry.Label;
            Detail = bibtexEntry.ApplyStyle(entryStyle);
        }

        /// <summary>
        /// The citation key associated with the Bibitem.
        /// </summary>
        public string CitationKey { get; set; }

        /// <summary>
        /// The label displayed in LaTeX.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The full bibitem detail which is displayed in the bibliography.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Formats the Bibitem as a string for the .bbl file.
        /// </summary>
        /// <returns>A a formatted bibitem string</returns>
        public override string ToString() => $"\\bibitem{(Label != null ? $"[{Label}]" : "")}{{{CitationKey}}}\r\n{Detail}";
    }
}