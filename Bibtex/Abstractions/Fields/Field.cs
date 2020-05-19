using Bibtex.Enumerations;

namespace Bibtex.Abstractions.Fields
{
    public abstract class Field
    {
        protected Field(FieldType type)
        {
            Type = type;
        }

        public FieldType Type { get; set; }

        public bool Bold { get; set; }

        public bool Italic { get; set; }

        public string Value { get; set; }
    }
}