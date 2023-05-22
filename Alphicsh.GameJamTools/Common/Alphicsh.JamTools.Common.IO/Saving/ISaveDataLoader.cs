namespace Alphicsh.JamTools.Common.IO.Saving
{
    public interface ISaveDataLoader<TModel, TData>
        where TData : class
    {
        TData? Load(TModel model);
    }
}
