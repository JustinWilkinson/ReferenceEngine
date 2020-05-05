namespace Bibtex.Abstractions.Entries
{
    public class Preamble
    {
        public string Content { get; set; }

        public Preamble(string content)
        {
            Content = content.Trim(' ', '"', '{', '}', '\t', '\r', '\n');
        }
    }
}