using Bibtex.Extensions;
using System.Collections.Generic;

namespace Bibtex.Abstractions.Entries
{
    /// <summary>
    /// Represents a String entry in a .bib file. Used to set variables inside the .bib file.
    /// </summary>
    public class StringEntry
    {
        /// <summary>
        /// The key value pair represented by the string entry.
        /// </summary>
        public KeyValuePair<string, string> Content { get; set; }

        /// <summary>
        /// Constructs a new StringEntry object from a string of the form "key=value".
        /// </summary>
        /// <param name="content">String of the form "key=value"</param>
        public StringEntry(string content)
        {
            var split = content.TrimIgnoredCharacters().Split('=', 2);
            Content = new KeyValuePair<string, string>(split[0].TrimIgnoredCharacters(), split[1].TrimIgnoredCharacters());
        }

        /// <summary>
        /// Formats the StringEntry as a .bib entry.
        /// </summary>
        /// <returns>The comment formatted as it would be in a .bib file.</returns>
        public override string ToString() => $"@COMMENT{{{Content.Key} = {{{Content.Value}}}}}";
    }
}