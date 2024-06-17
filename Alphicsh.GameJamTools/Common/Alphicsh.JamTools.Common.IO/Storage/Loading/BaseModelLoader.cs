﻿using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public abstract class BaseModelLoader<TInfo, TCore, TModel> : IModelLoader<TModel>
        where TInfo : class
        where TCore : class
        where TModel : class
    {
        private static FileDataManager FileDataManager { get; } = new FileDataManager();

        private IModelInfoReader<TInfo, TCore> InfoReader { get; }
        private IMapper<TInfo, TModel> InfoMapper { get; }
        private bool RecoverBeforeLoading { get; }

        public BaseModelLoader(
            IModelInfoReader<TInfo, TCore> infoReader,
            IMapper<TInfo, TModel> infoMapper,
            bool recoverBeforeLoading
            )
        {
            InfoReader = infoReader;
            InfoMapper = infoMapper;
            RecoverBeforeLoading = recoverBeforeLoading;
        }

        public async Task<TModel?> LoadFrom(FilePath location)
        {
            var modelInfo = await LoadModelInfo(location);
            if (modelInfo == null)
                return null;

            return InfoMapper.Map(modelInfo);
        }

        private async Task<TInfo?> LoadModelInfo(FilePath location)
        {
            var corePath = InfoReader.LocateCore(location);
            if (corePath == null)
                return null;

            var coreFile = await FileDataManager.LoadFile(corePath.Value);
            if (coreFile == null)
                return null;

            var coreData = InfoReader.DeserializeCore(coreFile);
            if (coreData == null)
                return null;

            var auxiliaryPaths = InfoReader.LocateAuxiliaryFiles(coreData);
            var infoBatch = await FileDataManager.LoadBatch(coreFile, auxiliaryPaths);
            return InfoReader.DeserializeModelInfo(infoBatch, coreData);
        }
    }
}
