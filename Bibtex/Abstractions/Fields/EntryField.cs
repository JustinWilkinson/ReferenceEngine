using Bibtex.Enumerations;

namespace Bibtex.Abstractions.Fields
{
    public class EntryField : Field
    {
        public EntryField() : base(FieldType.Field)
        {

        }

        public string Value { get; set; }
    }
}