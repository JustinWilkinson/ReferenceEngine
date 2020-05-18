using Bibtex.Abstractions;
using LatexReferences.Models.Formats;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatexReferences.Models.Format
{
    public class EntryStyle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        private string EntryPropertiesString { get; set; }

        private string OutputAuthorFormatString { get; set; }

        [NotMapped]
        public IEnumerable<EntryProperty> EntryProperties
        {
            get => EntryPropertiesString != null ? JsonConvert.DeserializeObject<IEnumerable<EntryProperty>>(EntryPropertiesString) : null;
            set => EntryPropertiesString = JsonConvert.SerializeObject(value);
        }

        [NotMapped]
        public OutputAuthorFormat OutputAuthorFormat
        {
            get => OutputAuthorFormatString != null ? JsonConvert.DeserializeObject<OutputAuthorFormat>(OutputAuthorFormatString) : null;
            set => OutputAuthorFormatString = JsonConvert.SerializeObject(value);
        }
    }
}