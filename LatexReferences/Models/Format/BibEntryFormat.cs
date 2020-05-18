using Bibtex.Enumerations;

namespace LatexReferences.Models.Format
{
    public class BibEntryFormat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EntryType EntryType { get; set; }

        public bool IncludeCitationKey { get; }

        public bool IncludeAddress { get; set; }

        public bool IncludeAnnote { get; set; }

        public bool IncludeAuthor { get; set; }

        public bool IncludeBooktitle { get; set; }

        public bool IncludeChapter { get; set; }

        public bool IncludeCrossReference { get; set; }

        public bool IncludeDOI { get; set; }

        public bool IncludeEdition { get; set; }

        public bool IncludeEditor { get; set; }

        public bool IncludeEmail { get; set; }

        public bool IncludeHowpublished { get; set; }

        public bool IncludeInstitution { get; set; }

        public bool IncludeJournal { get; set; }

        public bool IncludeKey { get; set; }

        public bool IncludeMonth { get; set; }

        public bool IncludeNumber { get; set; }

        public bool IncludeOrganization { get; set; }

        public bool IncludePages { get; set; }

        public bool IncludePublisher { get; set; }

        public bool IncludeSchool { get; set; }

        public bool IncludeSeries { get; set; }

        public bool IncludeTitle { get; set; }

        public bool IncludeType { get; set; }

        public bool IncludeVolume { get; set; }

        public bool IncludeYear { get; set; }
    }
}