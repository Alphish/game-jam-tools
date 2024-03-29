﻿using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Serialization
{
    public class JsonContentSerializer<TEntity> : IContentSerializer<TEntity>
    {
        // -----
        // Setup
        // -----

        protected JsonSerializerOptions SerializerOptions { get; }

        public JsonContentSerializer() : this(JsonIgnoreCondition.WhenWritingNull)
        {
        }

        public JsonContentSerializer(JsonIgnoreCondition ignoreCondition)
        {
            SerializerOptions = new JsonSerializerOptions()
            {
                // characters encoding
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

                // allowing flexible JSONs
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,

                // ignoring properties
                DefaultIgnoreCondition = ignoreCondition,
                IncludeFields = false,

                // property names
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                // indentation
                WriteIndented = true,
            };

            // converters
            SerializerOptions.Converters.Add(new FilePathJsonConverter());
        }

        // -------
        // Methods
        // -------

        public TEntity? Deserialize(string content)
        {
            return JsonSerializer.Deserialize<TEntity>(content, SerializerOptions);
        }

        public string Serialize(TEntity entity)
        {
            return JsonSerializer.Serialize(entity, SerializerOptions);
        }
    }
}
