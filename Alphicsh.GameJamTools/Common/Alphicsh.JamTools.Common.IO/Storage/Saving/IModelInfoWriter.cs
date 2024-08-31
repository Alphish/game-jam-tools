namespace Alphicsh.JamTools.Common.IO.Storage.Saving
{
    public interface IModelInfoWriter<TInfo> where TInfo : class
    {
        FileBatch SerializeModelInfo(TInfo info);
    }
}
