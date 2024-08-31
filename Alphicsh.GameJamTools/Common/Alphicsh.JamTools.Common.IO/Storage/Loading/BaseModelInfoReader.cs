using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public abstract class BaseModelInfoReader<TInfo, TCore> : IModelInfoReader<TInfo, TCore>
        where TInfo : class
        where TCore : class
    {
        public abstract FilePath? LocateCore(FilePath dataLocation);
        public abstract TCore? DeserializeCore(FileData coreFile);

        public abstract IEnumerable<FilePath> LocateAuxiliaryFiles(TCore coreData);
        public abstract TInfo DeserializeModelInfo(FileBatch fileBatch, TCore coreData);

        public virtual TInfo? GetFallbackInfo(FilePath dataLocation)
        {
            return null;
        }
    }
}
