using Bibtex.Abstractions;
using LatexReferences.Models.Fields;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatexReferences.Models
{
    public class EntryStyle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        private string FieldsString { get; set; }

        [NotMapped]
        public IEnumerable<Field> Fields
        {
            get => FieldsString != null ? JsonConvert.DeserializeObject<IEnumerable<Field>>(FieldsString) : null;
            set => FieldsString = JsonConvert.SerializeObject(value);
        }
    }
}