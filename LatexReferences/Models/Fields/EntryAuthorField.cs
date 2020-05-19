using Bibtex.Abstractions;

namespace LatexReferences.Models.Fields
{
    public class EntryAuthorField : Field
    {
        public EntryAuthorField() : base(FieldType.EntryAuthorField)
        {

        }

        public OutputAuthorFormat Format { get; set; }
    }
}