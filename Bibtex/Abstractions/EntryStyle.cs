using Bibtex.Abstractions.Fields;
using Bibtex.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibtex.Abstractions
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
        /// The fields to be extracted from the entry.
        /// </summary>
        [NotMapped]
        [JsonConverter(typeof(FieldConverter))]
        public IEnumerable<Field> Fields
        {
            get => FieldsJson != null ? JsonConvert.DeserializeObject<IEnumerable<Field>>(FieldsJson, new FieldConverter()) : new List<Field>();
            set
            {
                FieldsJson = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary>
        /// A JSON representation of the Fields - useful for entity framework.
        /// </summary>
        [JsonIgnore]
        public string FieldsJson { get; set; }

        /// <summary>
        /// The default styling.
        /// </summary>
        public static EntryStyle Default = new EntryStyle { Fields = new List<Field>() };
    }
}