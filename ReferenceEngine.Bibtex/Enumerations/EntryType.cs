namespace ReferenceEngine.Bibtex.Enumerations
{
    /// <summary>
    /// Represents the known Bibtex entry types: <a href="http://bib-it.sourceforge.net/help/fieldsAndEntryTypes.php"></a> found in a .bib file.
    /// </summary>
    public enum EntryType
    {
        /// <summary>
        /// Represents an Article entry, of the form @Article{}.
        /// </summary>
        Article,

        /// <summary>
        /// Represents a Book entry, of the form @Book{}.
        /// </summary>
        Book,

        /// <summary>
        /// Represents a Booklet entry, of the form @Booklet{}.
        /// </summary>
        Booklet,

        /// <summary>
        /// Represents a Comment entry, of the form @Comment{}.
        /// </summary>
        Comment,

        /// <summary>
        /// Represents a Conference entry, of the form @Conference{}.
        /// </summary>
        Conference,

        /// <summary>
        /// Represents an InBook entry, of the form @InBook{}.
        /// </summary>
        InBook,

        /// <summary>
        /// Represents an InCollection entry, of the form @InCollection{}.
        /// </summary>
        InCollection,

        /// <summary>
        /// Represents an InProceedings entry, of the form @InProceedings{}.
        /// </summary>
        InProceedings,

        /// <summary>
        /// Represents a Manual entry, of the form @Manual{}.
        /// </summary>
        Manual,

        /// <summary>
        /// Represents a MastersThesis entry, of the form @MastersThesis{}.
        /// </summary>
        MastersThesis,

        /// <summary>
        /// Represents a Misc entry, of the form @Misc{}.
        /// </summary>
        Misc,

        /// <summary>
        /// Represents a PhDThesis entry, of the form @PhDThesis{}.
        /// </summary>
        PhDThesis,

        /// <summary>
        /// Represents a Preamble entry, of the form @Preamble{}.
        /// </summary>
        Preamble,

        /// <summary>
        /// Represents a Proceedings entry, of the form @Proceedings{}.
        /// </summary>
        Proceedings,

        /// <summary>
        /// Represents a String entry, of the form @String{}.
        /// </summary>
        String,

        /// <summary>
        /// Represents a TechReport entry, of the form @TechReport{}.
        /// </summary>
        TechReport,

        /// <summary>
        /// Represents an Unpublished entry, of the form @Unpublished{}.
        /// </summary>
        Unpublished
    }
}