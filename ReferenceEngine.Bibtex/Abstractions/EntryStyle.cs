﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReferenceEngine.Bibtex.Abstractions.Fields;
using ReferenceEngine.Bibtex.Enumerations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReferenceEngine.Bibtex.Abstractions
{
    /// <summary>
    /// Represents the styling of an entry.
    /// </summary>
    public class EntryStyle
    {
        /// <summary>
        /// The Id of the entry style, useful for identification.
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// The name of the style.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of entry to apply this style to.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public EntryType Type { get; set; }

        /// <summary>
        /// Optional custom label template (used in inline citation). Can use any field value (including those not in the field list provided).
        /// These fields should referenced by name in the value, provided it is enclosed in braces, e.g. "{Author} {Year}". Can also use "{Index}" to use the citation index.
        /// If not provided, the index is used.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The fields to be extracted from the entry.
        /// </summary>
        [NotMapped]
        [JsonConverter(typeof(FieldConverter))]
        public IEnumerable<Field> Fields
        {
            get => FieldsJson != null ? JsonConvert.DeserializeObject<IEnumerable<Field>>(FieldsJson, new FieldConverter()) : new List<Field>();
            set => FieldsJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// A JSON representation of the Fields - useful for entity framework.
        /// </summary>
        [JsonIgnore]
        public string FieldsJson { get; set; }

        /// <summary>
        /// The default styling.
        /// </summary>
        public static EntryStyle Default = new EntryStyle
        {
            Fields = new List<Field>
            {
                new EntryField
                {
                    Value = "Title",
                    Suffix = ",",
                },
                new EntryAuthorField
                {
                    Format = new OutputAuthorFormat(),
                    Suffix = ","
                },
                new EntryField
                {
                    Value = "Year",
                    Suffix = "."
                }
            }
        };
    }
}