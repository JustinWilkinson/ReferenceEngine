using Bibtex.Enumerations;

namespace Bibtex.Abstractions.Fields
{
    public class ConstantField : Field
    {
        public ConstantField() : base(FieldType.Constant)
        {

        }

        public string Value { get; set; }
    }
}