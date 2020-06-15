using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReferenceEngine.Bibtex.Enumerations;
using System;
using System.Collections.Generic;

namespace ReferenceEngine.Bibtex.Abstractions.Fields
{
    /// <summary>
    /// Defines a custom JsonConverter for the Field Base class which deserializes a Field based on its type.
    /// </summary>
    public class FieldConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => typeof(Field) == objectType || typeof(Field).IsAssignableFrom(objectType);

        /// <inheritdoc />
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

        /// <summary>
        /// Always false, we do not need to Write JSON with this converter.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// This is not implemented, but will never be called as CanWrite returns false.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes the JObject to a Field based on its type.
        /// </summary>
        /// <param name="obj">JObject representing a Field.</param>
        /// <returns>The field represented by the provided JObject.</returns>
        /// <exception cref="ArgumentException">Thrown where the field type is not recognized.</exception>
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