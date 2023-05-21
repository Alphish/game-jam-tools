namespace Alphicsh.JamTools.Common.IO.Saving
{
    public interface ISaveDataExtractor<TModel, TData>
        where TModel : class
        where TData : class
    {
        TData ExtractData(TModel model);
    }
}
