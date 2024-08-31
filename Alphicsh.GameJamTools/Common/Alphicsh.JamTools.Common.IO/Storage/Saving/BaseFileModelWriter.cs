using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.JamTools.Common.IO.Storage.Saving
{
    public abstract class BaseFileModelWriter<TInfo> : IModelInfoWriter<TInfo>
        where TInfo : class
    {
        public FileBatch SerializeModelInfo(TInfo info)
        {
            var coreData = SerializeCore(info);
            var auxiliaryData = SerializeAuxiliaryInfo(info);
            return new FileBatch(coreData, auxiliaryData);
        }

        protected abstract FileData SerializeCore(TInfo info);

        protected virtual IEnumerable<FileData> SerializeAuxiliaryInfo(TInfo info)
        {
            return Enumerable.Empty<FileData>();
        }
    }
}
