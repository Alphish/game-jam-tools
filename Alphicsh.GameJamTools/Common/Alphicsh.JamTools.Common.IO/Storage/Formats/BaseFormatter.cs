namespace Alphicsh.JamTools.Common.IO.Storage.Formats
{
    public abstract class BaseFormatter<TEntity> : IFormatter<TEntity>
        where TEntity : class
    {
        public abstract string Format(TEntity entity);

        public FileData FormatToFile(FilePath path, TEntity entity)
        {
            var content = Format(entity);
            return new FileData(path, content);
        }

        public abstract TEntity? Parse(string content);

        public TEntity? ParseFromFile(FileData file)
        {
            return Parse(file.Content);
        }
    }
}
