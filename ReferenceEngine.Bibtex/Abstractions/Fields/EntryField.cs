using Bibtex.Enumerations;

namespace Bibtex.Abstractions.Fields
{
    /// <summary>
    /// Represents a field in a BibtexEntry the style file.
    /// </summary>
    public class EntryField : Field
    {
        /// <summary>
        /// Constructs a new EntryField instance.
        /// </summary>
        public EntryField() : base(FieldType.Field)
        {

        }

        /// <summary>
        /// Value of the Entry Field - this is the name of the property to use.
        /// </summary>
        public string Value { get; set; }
    }
}