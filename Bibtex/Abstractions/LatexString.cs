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
            string value = Value;
            if (Italic)
            {
                value = $"{{\\em {value}}}";
            }
            if (Bold)
            {
                value = $"\\textbf{{{value}}}";
            }
            return $"\\newblock {value}";
        }
    }
}