using Bibtex.Abstractions;
using Bibtex.Abstractions.Entries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bibtex
{
    public class BibliographyBuilder
    {
        private string _destination;

        private readonly List<Bibitem> _bibitems = new List<Bibitem>();

        public void AddEntries(IEnumerable<BibtexEntry> entries, Func<BibtexEntry, Bibitem> func) => _bibitems.AddRange(entries.Select(x => func(x)));

        public void SetDestination(string destinationPath) => _destination = destinationPath;

        public void Build()
        {
            if (_destination == null)
            {
                throw new ArgumentNullException("Bibliography destination path cannot be null!");
            }

            using var writer = new StreamWriter(_destination);
            foreach (var bibitem in _bibitems)
            {
                writer.WriteLine(bibitem.ToString());
            }
        }
    }
}