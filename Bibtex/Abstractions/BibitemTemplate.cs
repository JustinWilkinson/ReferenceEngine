using Bibtex.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bibtex.Abstractions
{
    public class BibitemTemplate
    {
        public string PropertyDelimiter { get; set; }

        public OutputAuthorFormat AuthorFormat { get; set; }

        public List<LatexString> IncludedProperties { get; set; }

        public static BibitemTemplate DefaultBook { get; } = new BibitemTemplate
        {
            PropertyDelimiter = ",\r\n",
            AuthorFormat = OutputAuthorFormat.Default,
            IncludedProperties = new List<LatexString>
            {
                new LatexString("Author"),
                new LatexString("Title") { Italic = true },
                new LatexString("Publisher"),
                new LatexString("Address"),
                new LatexString("Year")
            }
        };

        public static BibitemTemplate GetDefaultTemplate(EntryType type)
        {
            switch (type)
            {
                case EntryType.Article:
                    break;
                case EntryType.Book:
                    return DefaultBook;
                case EntryType.Booklet:
                    break;
                case EntryType.Comment:
                    return ThrowHelper(type);
                case EntryType.Conference:
                    break;
                case EntryType.InBook:
                    break;
                case EntryType.InCollection:
                    break;
                case EntryType.InProceedings:
                    break;
                case EntryType.Manual:
                    break;
                case EntryType.MastersThesis:
                    break;
                case EntryType.Misc:
                    break;
                case EntryType.PhDThesis:
                    break;
                case EntryType.Preamble:
                    return ThrowHelper(type);
                case EntryType.Proceedings:
                    break;
                case EntryType.String:
                    return ThrowHelper(type);
                case EntryType.TechReport:
                    break;
                case EntryType.Unpublished:
                    break;
                default:
                    return ThrowHelper(type);
            }

            return DefaultBook;
        }

        public BibitemTemplate Duplicate()
        {
            return new BibitemTemplate
            {
                PropertyDelimiter = PropertyDelimiter,
                AuthorFormat = AuthorFormat,
                IncludedProperties = IncludedProperties.Select(x => x.Duplicate()).ToList()
            };
        }

        private static BibitemTemplate ThrowHelper(EntryType entryType)
        {
            throw new ArgumentException($"EntryType '{entryType}' does not have a default template, as it is not rendered to the bibliography!");
        }
    }
}