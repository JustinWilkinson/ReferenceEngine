using Bibtex.Enumerations;

namespace Bibtex.Abstractions
{
    public class AuxEntry
    {
        public AuxEntryType Type { get; set; }

        public string Key { get; set; }

        public string Label { get; set; }

        public AuxEntry(AuxEntryType type)
        {
            Type = type;
        }
    }
}