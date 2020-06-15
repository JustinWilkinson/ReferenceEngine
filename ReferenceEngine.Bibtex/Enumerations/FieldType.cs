namespace Bibtex.Enumerations
{
    /// <summary>
    /// Represents a styling Field Type.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// Represents a constant, to be rendered as a literal latex string.
        /// </summary>
        Constant,

        /// <summary>
        /// Represents a field whose value is to be extracted from a BibtexEntry.
        /// </summary>
        Field,

        /// <summary>
        /// Represents a field whose value is to be extracted from the Author field of a BibtexEntry.
        /// </summary>
        AuthorField
    }
}