using Bibtex.Enumerations;

namespace Bibtex.Abstractions
{
    /// <summary>
    /// Represents an entry in the .aux file.
    /// </summary>
    public class AuxEntry
    {
        /// <summary>
        /// The type of the aux entry.
        /// </summary>
        public AuxEntryType Type { get; set; }

        /// <summary>
        /// The key associated to the citation.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The label to be used in the citation.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Constructs a new AuxEntry and assigns it the provided type.
        /// </summary>
        /// <param name="type"></param>
        public AuxEntry(AuxEntryType type)
        {
            Type = type;
        }
    }
}