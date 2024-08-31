using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Saving
{
    public class BaseModelSaver<TModel, TInfo> : IModelSaver<TModel>
        where TModel : class
        where TInfo : class
    {
        private static FileDataManager FileDataManager { get; } = new FileDataManager();

        private IMapper<TModel, TInfo> ModelMapper { get; }
        private IModelInfoWriter<TInfo> InfoWriter { get; }

        public BaseModelSaver(IMapper<TModel, TInfo> modelMapper, IModelInfoWriter<TInfo> infoWriter)
        {
            ModelMapper = modelMapper;
            InfoWriter = infoWriter;
        }

        public FileBatch PrepareFileBatch(TModel model)
        {
            var modelInfo = ModelMapper.Map(model);
            return InfoWriter.SerializeModelInfo(modelInfo);
        }

        public Task<FileBatch> PrepareFileBatchAsync(TModel model)
        {
            return Task.Run(() => PrepareFileBatch(model));
        }

        public async Task SaveFileBatch(FileBatch newBatch, FileBatch previousBatch)
        {
            await FileDataManager.WriteData(newBatch, previousBatch);
        }
    }
}
