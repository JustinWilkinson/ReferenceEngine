using Bibtex.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bibtex.Abstractions.Fields
{
    public abstract class Field
    {
        protected Field(FieldType type)
        {
            Type = type;
        }

        [JsonProperty(Order = -2)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType Type { get; set; }

        [JsonProperty(Order = -2)]
        public bool Bold { get; set; }

        [JsonProperty(Order = -2)]
        public bool Italic { get; set; }

        [JsonProperty(Order = -2)]
        public string Prefix { get; set; }

        [JsonProperty(Order = -2)]
        public string Suffix { get; set; }
    }
}