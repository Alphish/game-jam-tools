namespace Alphicsh.JamTools.Common.IO.Saving
{
    public interface IDataSaver<TData>
        where TData : class
    {
        void Save(TData data);
    }
}
