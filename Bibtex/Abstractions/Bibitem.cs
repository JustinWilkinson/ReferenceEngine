using System;
using System.Collections.Generic;
using System.Text;

namespace Bibtex.Abstractions
{
    public class Bibitem
    {
        public string CitationKey { get; set; }

        public string Label { get; set; }

        public string Detail { get; set; }

        public override string ToString() => $"\\bibitem{(Label != null ? $"[{Label}]" : "")}{{{CitationKey}}}\n{Detail}";
    }
}