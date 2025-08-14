using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public abstract class BaseModelInfoReader<TInfo, TCore> : IModelInfoReader<TInfo>
        where TInfo : class
        where TCore : class
    {
        private static FileDataManager FileDataManager { get; } = new FileDataManager();

        public async Task<TInfo?> LoadModelInfo(FilePath location, bool fixBeforeLoading)
        {
            var result = await DoLoad(location, fixBeforeLoading);
            return result ?? GetFallbackInfo(location);
        }

        private async Task<TInfo?> DoLoad(FilePath location, bool fixBeforeLoading)
        {
            var corePath = LocateCore(location);
            if (corePath == null)
                return null;

            if (fixBeforeLoading)
            {
                await FileDataManager.FixWriteData(corePath.Value);
            }

            var coreFile = await FileDataManager.LoadFile(corePath.Value);
            if (coreFile == null)
                return null;

            var coreData = DeserializeCore(coreFile);
            if (coreData == null)
                return null;

            var auxiliaryPaths = LocateAuxiliaryFiles(coreData);
            var infoBatch = await FileDataManager.LoadBatch(coreFile, auxiliaryPaths);
            return DeserializeModelInfo(infoBatch, coreData);
        }

        protected abstract FilePath? LocateCore(FilePath dataLocation);
        protected abstract TCore? DeserializeCore(FileData coreFile);

        protected abstract IEnumerable<FilePath> LocateAuxiliaryFiles(TCore coreData);
        protected abstract TInfo DeserializeModelInfo(FileBatch fileBatch, TCore coreData);

        protected virtual TInfo? GetFallbackInfo(FilePath dataLocation)
        {
            return null;
        }
    }
}
