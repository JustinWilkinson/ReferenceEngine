using Bibtex.Extensions;
using System.Collections.Generic;

namespace Bibtex.Abstractions.Entries
{
    public class StringEntry
    {
        public KeyValuePair<string, string> Content { get; set; }

        public StringEntry(string content)
        {
            var split = content.TrimIgnoredCharacters().Split('=', 2);
            Content = new KeyValuePair<string, string>(split[0].TrimIgnoredCharacters(), split[1].TrimIgnoredCharacters());
        }
    }
}