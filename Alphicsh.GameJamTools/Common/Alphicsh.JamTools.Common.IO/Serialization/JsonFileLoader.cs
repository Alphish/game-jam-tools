using System;
using System.IO;

namespace Alphicsh.JamTools.Common.IO.Serialization
{
    public class JsonFileLoader<TEntity>
        where TEntity : class
    {
        private JsonContentSerializer<TEntity> Serializer { get; } = new JsonContentSerializer<TEntity>();

        public TEntity? TryLoad(FilePath filePath)
        {
            if (filePath.IsRelative())
                throw new ArgumentException("The jam entry info can only be read from the absolute file path.", nameof(filePath));

            if (!filePath.HasFile())
                return null;

            var content = File.ReadAllText(filePath.Value);
            return Serializer.Deserialize(content);
        }
    }
}
