using System;
using System.IO;
using System.Text.Json.Serialization;

namespace Alphicsh.JamTools.Common.IO.Serialization
{
    public class JsonFileSaver<TEntity>
        where TEntity : class
    {
        private JsonContentSerializer<TEntity> Serializer { get; }

        public JsonFileSaver()
        {
            Serializer = new JsonContentSerializer<TEntity>(JsonIgnoreCondition.WhenWritingNull);
        }

        public JsonFileSaver(JsonIgnoreCondition ignoreCondition)
        {
            Serializer = new JsonContentSerializer<TEntity>(ignoreCondition);
        }

        public void Save(FilePath filePath, TEntity entity)
        {
            if (filePath.IsRelative())
                throw new ArgumentException("The entity information can only be saved to the absolute file path.", nameof(filePath));

            var content = Serializer.Serialize(entity);
            Directory.CreateDirectory(filePath.GetParentDirectoryPath().Value);
            File.WriteAllText(filePath.Value, content);
        }
    }
}
