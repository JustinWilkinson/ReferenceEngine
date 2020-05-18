using Bibtex.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bibtex.Abstractions.Entries
{
    public class BibtexEntry
    {
        public EntryType EntryType { get; }

        public string CitationKey { get; }

        public string Address { get; set; }

        public string Annote { get; set; }

        public string Author { get; set; }

        public string Booktitle { get; set; }

        public string Chapter { get; set; }

        public string CrossReference { get; set; }

        public string DOI { get; set; }

        public string Edition { get; set; }

        public string Editor { get; set; }

        public string Email { get; set; }

        public string HowPublished { get; set; }

        public string Institution { get; set; }

        public string Journal { get; set; }

        public string Key { get; set; }

        public string Month { get; set; }

        public string Number { get; set; }

        public string Organization { get; set; }

        public string Pages { get; set; }

        public string Publisher { get; set; }

        public string School { get; set; }

        public string Series { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Volume { get; set; }

        public string Year { get; set; }

        private static readonly Dictionary<string, Func<BibtexEntry, string>> _propertyGetters = new Dictionary<string, Func<BibtexEntry, string>>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, Action<BibtexEntry, string>> _propertySetters = new Dictionary<string, Action<BibtexEntry, string>>(StringComparer.OrdinalIgnoreCase);

        static BibtexEntry()
        {
            foreach (var property in typeof(BibtexEntry).GetProperties().Where(p => p.CanWrite && p.PropertyType == typeof(string)))
            {
                _propertyGetters.Add(property.Name, (Func<BibtexEntry, string>)Delegate.CreateDelegate(typeof(Func<BibtexEntry, string>), property.GetMethod));
                _propertySetters.Add(property.Name, (Action<BibtexEntry, string>)Delegate.CreateDelegate(typeof(Action<BibtexEntry, string>), property.SetMethod));
            }
        }

        public BibtexEntry(EntryType entryType, string citationKey, Dictionary<string, string> keyValuePairs)
        {
            EntryType = entryType;
            CitationKey = citationKey;

            if (keyValuePairs != null)
            {
                foreach (var kvp in keyValuePairs)
                {
                    if (_propertySetters.TryGetValue(kvp.Key, out var propertySetter))
                    {
                        propertySetter(this, kvp.Value);
                    }
                }
            }
        }

        public string GetDetailFromTemplate(BibitemTemplate template)
        {
            if (template is null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var templateToPopulate = template.Duplicate();

            foreach (var includedProperty in templateToPopulate.IncludedProperties)
            {
                if (_propertyGetters.TryGetValue(includedProperty.Value, out var propertyGetter))
                {
                    includedProperty.Value = includedProperty.Value.Equals("Author", StringComparison.OrdinalIgnoreCase) ? templateToPopulate.AuthorFormat.FormatAuthorField(propertyGetter(this)) : propertyGetter(this);
                }
            }

            return string.Join(templateToPopulate.PropertyDelimiter, templateToPopulate.IncludedProperties.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => x.ToString()));
        }
    }
}