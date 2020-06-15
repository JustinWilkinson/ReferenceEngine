namespace ReferenceEngine.Bibtex.Abstractions
{
    /// <summary>
    /// A wrapper for a string that has basic latex styling applied to it.
    /// </summary>
    public class LatexString
    {
        /// <summary>
        /// Constructs a new LatexString with the provided value.
        /// </summary>
        /// <param name="value"></param>
        public LatexString(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Specifies whether or not this string should be displayed in bold.
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// Specifies whether or not this string should be displayed in italic.
        /// </summary>
        public bool Italic { get; set; }

        /// <summary>
        /// Specifies whether or not this string should be displayed in quotes.
        /// </summary>
        public bool Enquote { get; set; }

        /// <summary>
        /// The actual contents of the LatexString.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Formats the value of the string with the necessary bold/italic modifiers in LaTeX.
        /// </summary>
        /// <returns>A formatted latex string.</returns>
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
            if (Enquote)
            {
                value = $"\\enquote{{{value}}}";
            }

            return value;
        }
    }
}