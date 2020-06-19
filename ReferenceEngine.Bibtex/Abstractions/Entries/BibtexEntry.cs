using ReferenceEngine.Bibtex.Abstractions.Fields;
using ReferenceEngine.Bibtex.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SC = System.StringComparison;

namespace ReferenceEngine.Bibtex.Abstractions.Entries
{
    /// <summary>
    /// Represents a citeable Bibtex Entry, such as an Article, or Book in a .bib file.
    /// </summary>
    public class BibtexEntry
    {
        #region Constants
        private const string ENTRY_TYPE = "EntryType";
        private const string CITATION_KEY = "CitationKey";
        #endregion

        #region Static
        /// <summary>
        /// Internal dictionary of property getters.
        /// </summary>
        private static readonly Dictionary<string, Func<BibtexEntry, string>> _propertyGetters = new Dictionary<string, Func<BibtexEntry, string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Internal dictionary of property setters.
        /// </summary>
        private static readonly Dictionary<string, Action<BibtexEntry, string>> _propertySetters = new Dictionary<string, Action<BibtexEntry, string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Static constructor. Uses reflection as a one off to build a dicitionary of property accessors.
        /// </summary>
        static BibtexEntry()
        {
            foreach (var property in typeof(BibtexEntry).GetProperties().Where(p => p.CanWrite && p.PropertyType == typeof(string) && p.GetIndexParameters().Length == 0))
            {
                _propertyGetters.Add(property.Name, (Func<BibtexEntry, string>)Delegate.CreateDelegate(typeof(Func<BibtexEntry, string>), property.GetMethod));
                _propertySetters.Add(property.Name, (Action<BibtexEntry, string>)Delegate.CreateDelegate(typeof(Action<BibtexEntry, string>), property.SetMethod));
            }
        }
        #endregion

        /// <summary>
        /// Internal dictionary to support indexer. Contains all property values specific to the entry.
        /// </summary>
        private readonly Dictionary<string, string> _keyValuePairs;

        /// <summary>
        /// Constructs a new BibtexEntry of the specified type with the given citation key.
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="citationKey"></param>
        public BibtexEntry(EntryType entryType, string citationKey)
        {
            EntryType = entryType;
            CitationKey = citationKey;
            _keyValuePairs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Constructs of the specified type with the provided citation key and key values from the dictionary.
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="citationKey"></param>
        /// <param name="keyValuePairs"></param>
        public BibtexEntry(EntryType entryType, string citationKey, Dictionary<string, string> keyValuePairs) : this(entryType, citationKey)
        {
            if (keyValuePairs != null)
            {
                foreach (var kvp in keyValuePairs)
                {
                    if (_propertySetters.TryGetValue(kvp.Key, out var propertySetter))
                    {
                        propertySetter(this, kvp.Value);
                    }

                    _keyValuePairs.Add(kvp.Key, kvp.Value);
                }
            }
        }

        #region Read Only Properties
        /// <summary>
        /// The type of the entry.
        /// </summary>
        public EntryType EntryType { get; }

        /// <summary>
        /// The citation key associated with this entry.
        /// </summary>
        public string CitationKey { get; }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Address field for the entry.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Annote field for the entry.
        /// </summary>
        public string Annote { get; set; }

        /// <summary>
        /// Gets or sets the Author field for the entry.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the Booktitle field for the entry.
        /// </summary>
        public string Booktitle { get; set; }

        /// <summary>
        /// Gets or sets the Chapter field for the entry.
        /// </summary>
        public string Chapter { get; set; }

        /// <summary>
        /// Gets or sets the CrossReference field for the entry.
        /// </summary>
        public string CrossReference { get; set; }

        /// <summary>
        /// Gets or sets the DOI field for the entry.
        /// </summary>
        public string DOI { get; set; }

        /// <summary>
        /// Gets or sets the Edition field for the entry.
        /// </summary>
        public string Edition { get; set; }

        /// <summary>
        /// Gets or sets the Editor field for the entry.
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// Gets or sets the Email field for the entry.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the HowPublished field for the entry.
        /// </summary>
        public string HowPublished { get; set; }

        /// <summary>
        /// Gets or sets the Institution field for the entry.
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// Gets or sets the Journal field for the entry.
        /// </summary>
        public string Journal { get; set; }

        /// <summary>
        /// Gets or sets the Key field for the entry.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the Month field for the entry.
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the Number field for the entry.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the Organization field for the entry.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the Pages field for the entry.
        /// </summary>
        public string Pages { get; set; }

        /// <summary>
        /// Gets or sets the Publisher field for the entry.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the School field for the entry.
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// Gets or sets the Series field for the entry.
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the Title field for the entry.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Type field for the entry.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Volume field for the entry.
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// Gets or sets the Year field for the entry.
        /// </summary>
        public string Year { get; set; }
        #endregion

        /// <summary>
        /// Case insenstive indexer which gets or sets the value of the named property.
        /// Values which are set which are not properties will be added to the internal dictionary and so will be accessible anyway.
        /// Note that the EntryType and CitationKey are readonly, and will throw an exception when trying to set from here.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>The value of the field or if the field 
        /// does not exist, then null.</returns>
        public string this[string propertyName]
        {
            get
            {
                if (propertyName.Equals(ENTRY_TYPE, SC.OrdinalIgnoreCase))
                {
                    return EntryType.ToString();
                }
                else if (propertyName.Equals(CITATION_KEY, SC.OrdinalIgnoreCase))
                {
                    return CitationKey;
                }
                else if (_propertyGetters.TryGetValue(propertyName, out var getter))
                {
                    return getter(this);
                }

                return _keyValuePairs.TryGetValue(propertyName, out var value) ? value : null;
            }
            set
            {
                if (propertyName.Equals(ENTRY_TYPE, SC.OrdinalIgnoreCase) || propertyName.Equals(CITATION_KEY, SC.OrdinalIgnoreCase))
                {
                    throw new MemberAccessException($"Cannot set read-only property: '{propertyName}'");
                }

                if (_keyValuePairs.ContainsKey(propertyName))
                {
                    _keyValuePairs[propertyName] = value;
                }
                else
                {
                    _keyValuePairs.Add(propertyName, value);
                }

                if (_propertySetters.TryGetValue(propertyName, out var setter))
                {
                    setter(this, value);
                }
            }
        }

        /// <summary>
        /// Formats the BibtexEntry as a styled string containing the styled information.
        /// </summary>
        /// <param name="style">Style to apply.</param>
        /// <returns>A string LaTeX formatted string with styling.</returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Styles the BibtexEntry's label according to the provided style.
        /// </summary>
        /// <param name="style">Style to apply.</param>
        /// <returns>A string LaTeX formatted string with styling.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetStyledLabel(EntryStyle style, int index)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }
            
            if (style.Label != null)
            {
                style.Label = style.Label.Replace("{Index}", index.ToString(), SC.OrdinalIgnoreCase).Replace($"{{{CITATION_KEY}}}", CitationKey, SC.OrdinalIgnoreCase).Replace($"{{{ENTRY_TYPE}}}", EntryType.ToString(), SC.OrdinalIgnoreCase);
                
                foreach (var property in _keyValuePairs)
                {
                    style.Label = style.Label.Replace($"{{{property.Key}}}", property.Value, SC.OrdinalIgnoreCase);
                }

                return style.Label;
            }

            return index.ToString();
        }

        /// <summary>
        /// Formats the BibtexEntry as a .bib entry.
        /// </summary>
        /// <returns>The BibtexEntry formatted as it would be in a .bib file.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"@{EntryType}{{{CitationKey},\r\n");
            foreach (var kvp in _keyValuePairs)
            {
                sb.Append($"\t{kvp.Key} = {{{kvp.Value}}},\r\n");
            }
            sb.Length -= 3; // Trim trailing comma, carriage return and line feed.
            sb.Append('}');

            return sb.ToString();
        }
    }
}