﻿using Bibtex.Abstractions.Fields;
using Bibtex.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public string ApplyStyle(EntryStyle style)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            var builder = new StringBuilder();

            var isFirst = true;
            foreach (var field in style.Fields)
            {
                string text = null;

                switch (field.Type)
                {
                    case FieldType.Constant:
                        text = (field as ConstantField).Value;
                        break;
                    case FieldType.Field:
                        _propertyGetters.TryGetValue((field as EntryField).Value, out var propertyGetter);
                        text = propertyGetter(this);
                        break;
                    case FieldType.AuthorField:
                        text = (field as EntryAuthorField).Format.FormatAuthorField(Author);
                        break;
                }

                if (text != null)
                {
                    if (field.Prefix != null)
                    {
                        builder.Append(field.Prefix);
                    }
                    else
                    {
                        if (!isFirst)
                        {
                            builder.Append(" ");
                        }
                        else
                        {
                            isFirst = false;
                        }
                    }
                    builder.Append(new LatexString(text) { Bold = field.Bold, Italic = field.Italic });
                    builder.Append(field.Suffix);
                }
            }

            return builder.ToString();
        }
    }
}