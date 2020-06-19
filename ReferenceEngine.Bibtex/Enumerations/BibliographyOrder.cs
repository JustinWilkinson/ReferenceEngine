namespace ReferenceEngine.Bibtex.Enumerations
{
    /// <summary>
    /// Represents the ordering of entries in the bibliography.
    /// </summary>
    public enum BibliographyOrder
    {
        /// <summary>
        /// In order of appearance (the default).
        /// </summary>
        Appearance,
        
        /// <summary>
        /// Order by the last name of the first author (entries without authors will be ordered by title).
        /// </summary>
        AuthorLastName,

        /// <summary>
        /// Order by title.
        /// </summary>
        Title
    }
}