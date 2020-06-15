using Bibtex.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bibtex.Abstractions.Fields
{
    /// <summary>
    /// Abstract base class for BibtexEntry Fields in the style file.
    /// </summary>
    public abstract class Field
    {
        /// <summary>
        /// Base constructor for the Field - defines the type of the object.
        /// </summary>
        /// <param name="type"></param>
        protected Field(FieldType type)
        {
            Type = type;
        }

        /// <summary>
        /// The type of the Field.
        /// </summary>
        [JsonProperty(Order = -2)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType Type { get; }

        /// <summary>
        /// Determines whether this field should be written to the bibliography in bold.
        /// </summary>
        [JsonProperty(Order = -2)]
        public bool Bold { get; set; }

        /// <summary>
        /// Determines whether this field should be written to the bibliography in italic.
        /// </summary>
        [JsonProperty(Order = -2)]
        public bool Italic { get; set; }

        /// <summary>
        /// Defines a custom prefix to prepend to the field when writing to the bibliography. If this is not specified the default is to use a space to separate adjacent fields.
        /// This can be useful for seperating fields with a comma or a full stop, for example, or when used in conjunction with the Suffix property, to wrap the field in quotes or brackets.
        /// </summary>
        [JsonProperty(Order = -2)]
        public string Prefix { get; set; }

        /// <summary>
        /// Defines a custom suffix to append to the field when writing to the bibliography.
        /// This can be useful for seperating fields with a comma or a full stop, for example, or when used in conjunction with the Prefix property, to wrap the field in quotes or brackets.
        /// </summary>
        [JsonProperty(Order = -2)]
        public string Suffix { get; set; }
    }
}