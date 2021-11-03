using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO
{
    // ------------
    // Non-nullable
    // ------------

    public class FilePathJsonConverter : JsonConverter<FilePath>
    {
        public override FilePath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return FilePath.From(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, FilePath value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }

    // --------
    // Nullable
    // --------

    public class NullableFilePathJsonConverter : JsonConverter<FilePath?>
    {
        public override FilePath? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return FilePath.FromNullable(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, FilePath? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();

            writer.WriteStringValue(value!.Value.Value); // that's a lot of values
        }
    }
}
