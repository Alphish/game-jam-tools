using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO
{
    public class JsonContentSerializer<TEntity> : IContentSerializer<TEntity>
    {
        // -----
        // Setup
        // -----

        protected JsonSerializerOptions SerializerOptions { get; }

        public JsonContentSerializer()
        {
            SerializerOptions = new JsonSerializerOptions()
            {
                // allowing flexible JSONs
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,

                // ignoring properties
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IncludeFields = false,
                IgnoreReadOnlyProperties = true,

                // property names
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                // indentation
                WriteIndented = true,

            };

            // converters
            SerializerOptions.Converters.Add(new FilePathJsonConverter());
            SerializerOptions.Converters.Add(new NullableFilePathJsonConverter());
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
