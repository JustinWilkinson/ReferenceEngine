using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatexReferences.Models.Fields
{
    public abstract class Field 
    {
        protected Field (FieldType type)
        {
            FieldType = type;
        }

        public FieldType FieldType { get; set; }

        public bool Bold { get; set; }

        public bool Italic { get; set; }

        public string Value { get; set; }
    }

    public enum FieldType
    {
        Constant,
        EntryField,
        EntryAuthorField
    }
}