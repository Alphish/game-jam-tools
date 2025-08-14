using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public abstract class SingleFileModelReader<TInfo> : BaseModelInfoReader<TInfo, TInfo>
        where TInfo : class
    {
        protected override IEnumerable<FilePath> LocateAuxiliaryFiles(TInfo coreData)
        {
            return Enumerable.Empty<FilePath>();
        }

        protected override TInfo DeserializeModelInfo(FileBatch fileBatch, TInfo coreData)
        {
            return coreData;
        }
    }
}
