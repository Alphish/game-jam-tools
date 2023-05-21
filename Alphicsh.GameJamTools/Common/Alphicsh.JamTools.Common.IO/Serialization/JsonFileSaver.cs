using System;
using System.IO;

namespace Alphicsh.JamTools.Common.IO.Serialization
{
    public class JsonFileSaver<TEntity>
        where TEntity : class
    {
        private JsonContentSerializer<TEntity> Serializer { get; } = new JsonContentSerializer<TEntity>();

        public void Save(FilePath filePath, TEntity entity)
        {
            if (filePath.IsRelative())
                throw new ArgumentException("The jam entry info can only be saved to the absolute file path.", nameof(filePath));

            var content = Serializer.Serialize(entity);
            File.WriteAllText(filePath.Value, content);
        }
    }
}
