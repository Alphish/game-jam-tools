namespace Alphicsh.JamTools.Common.IO
{
    public interface IContentSerializer<TEntity>
    {
        string Serialize(TEntity entity);
        TEntity? Deserialize(string content);
    }
}
