using Bibtex.Abstractions.Entries;
using System;

namespace Bibtex.Abstractions
{
    public class Bibitem
    {
        public Bibitem (AuxEntry auxEntry, BibtexEntry bibtexEntry, BibitemTemplate bibitemTemplate)
        {
            if (auxEntry is null)
            {
                throw new ArgumentNullException(nameof(auxEntry));
            }
            if (bibtexEntry is null)
            {
                throw new ArgumentNullException(nameof(bibtexEntry));
            }
            if (bibitemTemplate is null)
            {
                throw new ArgumentNullException(nameof(bibitemTemplate));
            }

            CitationKey = auxEntry.Key;
            Label = auxEntry.Label;
            Detail = bibtexEntry.GetDetailFromTemplate(bibitemTemplate);
        }

        public string CitationKey { get; set; }

        public string Label { get; set; }

        public string Detail { get; set; }

        public override string ToString() => $"\\bibitem{(Label != null ? $"[{Label}]" : "")}{{{CitationKey}}}\r\n{Detail}";
    }
}