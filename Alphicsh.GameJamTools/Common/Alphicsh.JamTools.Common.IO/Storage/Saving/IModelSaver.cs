using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Saving
{
    public interface IModelSaver<TModel> where TModel : class
    {
        FileBatch PrepareFileBatch(TModel model);
        Task<FileBatch> PrepareFileBatchAsync(TModel model);

        Task SaveFileBatch(FileBatch newBatch, FileBatch previousBatch);
    }
}