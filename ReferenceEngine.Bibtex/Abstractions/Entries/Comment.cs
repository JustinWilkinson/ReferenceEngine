using Bibtex.Extensions;

namespace Bibtex.Abstractions.Entries
{
    /// <summary>
    /// Represents a comment entry in a .bib file. These values are ignored by a bibtex parser but can be helpful for a reader.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Comment text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Constructs a Comment from the provided text.
        /// </summary>
        /// <param name="text">Text to place in the comment.</param>
        public Comment(string text)
        {
            Text = text.TrimIgnoredCharacters();
        }

        /// <summary>
        /// Formats the Comment as a .bib entry.
        /// </summary>
        /// <returns>The Comment formatted as it would be in a .bib file.</returns>
        public override string ToString() => $"@COMMENT{{{Text}}}";
    }
}