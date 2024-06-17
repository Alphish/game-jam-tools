using System.Threading.Tasks;

namespace Alphicsh.JamTools.Common.IO.Storage.Loading
{
    public interface IModelLoader<TModel>
    {
        Task<TModel?> LoadFrom(FilePath location);
    }
}
