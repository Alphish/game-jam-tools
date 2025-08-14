using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public abstract class BaseModelLoader<TInfo, TCore, TModel> : IModelLoader<TModel>
        where TInfo : class
        where TCore : class
        where TModel : class
    {
        private IModelInfoReader<TInfo> InfoReader { get; }
        private IMapper<TInfo, TModel> InfoMapper { get; set; }
        private bool FixBeforeLoading { get; }

        public BaseModelLoader(
            IModelInfoReader<TInfo> infoReader,
            IMapper<TInfo, TModel> infoMapper,
            bool fixBeforeLoading
            )
        {
            InfoReader = infoReader;
            InfoMapper = infoMapper;
            FixBeforeLoading = fixBeforeLoading;
        }

        public async Task<TModel?> LoadFrom(FilePath location)
        {
            var modelInfo = await InfoReader.LoadModelInfo(location, FixBeforeLoading);
            if (modelInfo == null)
                return null;

            return InfoMapper.Map(modelInfo);
        }
    }
}
