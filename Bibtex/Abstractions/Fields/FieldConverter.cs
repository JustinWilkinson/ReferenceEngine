using Bibtex.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Bibtex.Abstractions.Fields
{
    public class FieldConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(Field) == objectType || typeof(Field).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var fields = new List<Field>();
                var array = JArray.Load(reader);

                foreach (JObject obj in array)
                {
                    fields.Add(GetField(obj));
                }

                return fields;
            }
            else
            {
                return GetField(JObject.Load(reader));
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }

        private Field GetField(JObject obj)
        {
            if (Enum.TryParse<FieldType>(obj["Type"].ToString(), true, out var type))
            {
                return type switch
                {
                    FieldType.Constant => JsonConvert.DeserializeObject<ConstantField>(obj.ToString()),
                    FieldType.AuthorField => JsonConvert.DeserializeObject<EntryAuthorField>(obj.ToString()),
                    FieldType.Field => JsonConvert.DeserializeObject<EntryField>(obj.ToString()),
                    _ => throw new ArgumentException($"Unrecognized Type {obj["Type"]}")
                };
            }

            throw new ArgumentException($"Unrecognized Type {obj["Type"]}");
        }
    }
}