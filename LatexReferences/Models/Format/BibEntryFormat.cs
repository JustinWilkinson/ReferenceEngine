using Bibtex.Enumerations;
using System.ComponentModel;

namespace LatexReferences.Models.Format
{
    public class BibEntryFormat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Entry Type")]
        public EntryType EntryType { get; set; }

        [DisplayName("Include Citation Key")]
        public bool IncludeCitationKey { get; }

        [DisplayName("Include Address")]
        public bool IncludeAddress { get; set; }

        [DisplayName("Include Annote")]
        public bool IncludeAnnote { get; set; }

        [DisplayName("Include Author")]
        public bool IncludeAuthor { get; set; }

        [DisplayName("Include Booktitle")]
        public bool IncludeBooktitle { get; set; }

        [DisplayName("Include Chapter")]
        public bool IncludeChapter { get; set; }

        [DisplayName("Include Cross Reference")]
        public bool IncludeCrossReference { get; set; }

        [DisplayName("Include DOI")]
        public bool IncludeDOI { get; set; }

        [DisplayName("Include Edition")]
        public bool IncludeEdition { get; set; }

        [DisplayName("Include Editor")]
        public bool IncludeEditor { get; set; }

        [DisplayName("Include Email")]
        public bool IncludeEmail { get; set; }

        [DisplayName("Include How Published")]
        public bool IncludeHowPublished { get; set; }

        [DisplayName("Include Institution")]
        public bool IncludeInstitution { get; set; }

        [DisplayName("Include Journal")]
        public bool IncludeJournal { get; set; }

        [DisplayName("Include Key")]
        public bool IncludeKey { get; set; }

        [DisplayName("Include Month")]
        public bool IncludeMonth { get; set; }

        [DisplayName("Include Number")]
        public bool IncludeNumber { get; set; }

        [DisplayName("Include Organization")]
        public bool IncludeOrganization { get; set; }

        [DisplayName("Include Pages")]
        public bool IncludePages { get; set; }

        [DisplayName("Include Publisher")]
        public bool IncludePublisher { get; set; }

        [DisplayName("Include School")]
        public bool IncludeSchool { get; set; }

        [DisplayName("Include Series")]
        public bool IncludeSeries { get; set; }

        [DisplayName("Include Title")]
        public bool IncludeTitle { get; set; }

        [DisplayName("Include Type")]
        public bool IncludeType { get; set; }

        [DisplayName("Include Volume")]
        public bool IncludeVolume { get; set; }

        [DisplayName("Include Year")]
        public bool IncludeYear { get; set; }
    }
}