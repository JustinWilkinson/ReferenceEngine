namespace ReferenceEngine.Bibtex.Enumerations
{
    /// <summary>
    /// Enumerates the possible types of AuxEntry.
    /// </summary>
    public enum AuxEntryType
    {
        /// <summary>
        /// An entry of the form \relax. This is ignored.
        /// </summary>
        Relax,

        /// <summary>
        /// An entry of the form \bibstyle{}. This is ignored.
        /// </summary>
        Bibstyle,

        /// <summary>
        /// An entry of the form \citation{}.
        /// </summary>
        Citation,

        /// <summary>
        /// An entry of the form \bibcite{}.
        /// </summary>
        Bibcite,

        /// <summary>
        /// An entry of the form \bibdata{}.
        /// </summary>
        Bibdata
    }
}