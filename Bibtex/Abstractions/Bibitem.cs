using Bibtex.Abstractions.Entries;
using System;

namespace Bibtex.Abstractions
{
    public class Bibitem
    {
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

            CitationKey = auxEntry.Key;
            Label = auxEntry.Label;
            Detail = bibtexEntry.ApplyStyle(entryStyle);
        }

        public string CitationKey { get; set; }

        public string Label { get; set; }

        public string Detail { get; set; }

        public override string ToString() => $"\\bibitem{(Label != null ? $"[{Label}]" : "")}{{{CitationKey}}}\r\n{Detail}";
    }
}