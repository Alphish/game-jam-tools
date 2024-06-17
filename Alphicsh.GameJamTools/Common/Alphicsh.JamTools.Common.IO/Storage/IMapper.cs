namespace Alphicsh.JamTools.Common.IO.Storage
{
    public interface IMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        TTarget Map(TSource info);
    }
}
