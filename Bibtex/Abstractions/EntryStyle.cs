using Bibtex.Abstractions.Fields;
using Bibtex.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibtex.Abstractions
{
    public class EntryStyle
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EntryType Type { get; set; }

        [JsonIgnore]
        internal string FieldsString { get; set; }

        [NotMapped]
        [JsonConverter(typeof(FieldConverter))]
        public IEnumerable<Field> Fields
        {
            get => FieldsString != null ? JsonConvert.DeserializeObject<IEnumerable<Field>>(FieldsString, new FieldConverter()) : new List<Field>();
            set
            {
                FieldsString = JsonConvert.SerializeObject(value);
                System.Diagnostics.Debug.WriteLine(FieldsString);
            }
        }

        public static EntryStyle Default = new EntryStyle { Fields = new List<Field>() };
    }
}