using ReferenceEngine.Bibtex.Enumerations;

namespace ReferenceEngine.Bibtex.Abstractions.Fields
{
    /// <summary>
    /// Represents the Author Field of a BibtexEntry in the style file, as these have more complex style rules.
    /// </summary>
    public class EntryAuthorField : Field
    {
        /// <summary>
        /// Constructs a new EntryAuthorField instance.
        /// </summary>
        public EntryAuthorField() : base(FieldType.AuthorField)
        {

        }

        /// <summary>
        /// The format for the author field.
        /// </summary>
        public OutputAuthorFormat Format { get; set; }
    }
}