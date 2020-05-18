namespace LatexReferences.Models.Formats
{
    public class EntryProperty
    {
        public EntryPropertyType EntryPropertyType { get; set; }

        public bool Bold { get; set; }

        public bool Italic { get; set; }

        public string Value { get; set; }
    }

    public enum EntryPropertyType
    {
        Constant,
        FromDatabase
    }
}