using Bibtex.Enumerations;

namespace Bibtex.Abstractions.Fields
{
    public class EntryAuthorField : Field
    {
        public EntryAuthorField() : base(FieldType.AuthorField)
        {

        }

        public OutputAuthorFormat Format { get; set; }
    }
}