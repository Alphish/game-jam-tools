using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public class LoadModel<TModel>
        where TModel : class
    {
        private IModelLoader<TModel> ModelLoader { get; set; }

        public TModel Model { get; private set; }
        private TModel BlankModel { get; }

        public bool IsLoading { get; private set; }
        public bool IsReady => Model != BlankModel;

        public LoadModel(IModelLoader<TModel> modelLoader, TModel blankModel)
        {
            ModelLoader = modelLoader;

            BlankModel = blankModel;
            Model = blankModel;
        }

        public void ReplaceLoader(IModelLoader<TModel> modelLoader)
        {
            ModelLoader = modelLoader;
        }

        public async Task LoadFrom(FilePath location)
        {
            IsLoading = true;

            var loadedModel = await ModelLoader.LoadFrom(location);
            if (IsLoading)
            {
                Model = loadedModel ?? BlankModel;
                IsLoading = false;
            }
        }

        public void Close()
        {
            Model = BlankModel;
            IsLoading = false;
        }
    }
}
