using Bibtex.Enumerations;

namespace Bibtex.Abstractions.Fields
{
    public abstract class Field
    {
        protected Field(FieldType type)
        {
            FieldType = type;
        }

        public FieldType FieldType { get; set; }

        public bool Bold { get; set; }

        public bool Italic { get; set; }

        public string Value { get; set; }
    }
}