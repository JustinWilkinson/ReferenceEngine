using ReferenceEngine.Bibtex.Extensions;

namespace ReferenceEngine.Bibtex.Abstractions.Entries
{
    /// <summary>
    /// Represents a preamble entry in a .bib file.
    /// </summary>
    public class Preamble
    {
        /// <summary>
        /// A string containing the preamble content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Constructs a new Preamble object with the specified content.
        /// </summary>
        /// <param name="content">Contents of the preamble entry.</param>
        public Preamble(string content)
        {
            Content = content.TrimIgnoredCharacters();
        }

        /// <summary>
        /// Formats the preamble as a .bib entry.
        /// </summary>
        /// <returns>The preamble formatted as it would be in a .bib file.</returns>
        public override string ToString() => $"@PREAMBLE{Content}";
    }
}