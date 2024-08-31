namespace Alphicsh.JamTools.Common.IO.Storage.Formats
{
    public interface IFormatter<TEntity>
        where TEntity : class
    {
        string Format(TEntity entity);
        FileData FormatToFile(FilePath path, TEntity entity);

        TEntity? Parse(string content);
        TEntity? ParseFromFile(FileData fileData);
    }
}
