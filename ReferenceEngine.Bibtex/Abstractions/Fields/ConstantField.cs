using ReferenceEngine.Bibtex.Enumerations;

namespace ReferenceEngine.Bibtex.Abstractions.Fields
{
    /// <summary>
    /// Represents a constant field in a BibtexEntry in the style file.
    /// </summary>
    public class ConstantField : Field
    {
        /// <summary>
        /// Constructs a new ConstantField instance.
        /// </summary>
        public ConstantField() : base(FieldType.Constant)
        {

        }

        /// <summary>
        /// Value of the constant field.
        /// </summary>
        public string Value { get; set; }
    }
}