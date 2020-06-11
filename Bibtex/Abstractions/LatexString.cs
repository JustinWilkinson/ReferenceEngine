namespace Bibtex.Abstractions
{
    public class LatexString
    {
        public LatexString(string value)
        {
            Value = value;
        }

        public bool Bold { get; set; }

        public bool Italic { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                return "";
            }

            string value = Value;

            if (Italic)
            {
                value = $"\\emph{{{value}}}";
            }
            if (Bold)
            {
                value = $"\\textbf{{{value}}}";
            }

            return value;
        }
    }
}