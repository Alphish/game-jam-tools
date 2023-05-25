namespace Alphicsh.JamTools.Common.IO.Serialization
{
    public interface IContentSerializer<TEntity>
    {
        string Serialize(TEntity entity);
        TEntity? Deserialize(string content);
    }
}
