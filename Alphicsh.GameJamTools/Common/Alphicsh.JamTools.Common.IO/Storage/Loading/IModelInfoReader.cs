using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public interface IModelInfoReader<TInfo>
        where TInfo : class
    {
        Task<TInfo?> LoadModelInfo(FilePath location, bool fixBeforeLoading);
    }
}
