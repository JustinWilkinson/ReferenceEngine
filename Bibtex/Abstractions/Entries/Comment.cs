namespace Bibtex.Abstractions.Entries
{
    public class Comment
    {
        public string Text { get; set; }

        public Comment(string text)
        {
            Text = text.Trim(' ', '"', '{', '}', '\t', '\r', '\n');
        }
    }
}