using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public interface IModelInfoReader<TInfo, TCore>
        where TInfo : class
        where TCore : class
    {
        FilePath? LocateCore(FilePath dataLocation);
        TCore? DeserializeCore(FileData coreFile);

        IEnumerable<FilePath> LocateAuxiliaryFiles(TCore coreData);
        TInfo? DeserializeModelInfo(FileBatch fileBatch, TCore coreData);
    }
}
