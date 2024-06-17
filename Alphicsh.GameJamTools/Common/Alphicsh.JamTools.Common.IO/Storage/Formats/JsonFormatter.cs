using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using Alphicsh.JamTools.Common.IO.Serialization;

namespace Alphicsh.JamTools.Common.IO.Storage.Formats
{
    public class JsonFormatter<TEntity> : BaseFormatter<TEntity>
        where TEntity : class
    {
        // -----
        // Setup
        // -----

        protected JsonSerializerOptions SerializerOptions { get; }

        public JsonFormatter() : this(JsonIgnoreCondition.WhenWritingNull)
        {
        }

        public JsonFormatter(JsonIgnoreCondition ignoreCondition)
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

        // --------------
        // Implementation
        // --------------

        public override string Format(TEntity entity)
        {
            return JsonSerializer.Serialize(entity, SerializerOptions);
        }

        public override TEntity? Parse(string content)
        {
            try
            {
                return JsonSerializer.Deserialize<TEntity>(content, SerializerOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
