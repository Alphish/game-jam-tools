using System.Collections.Specialized;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public interface ICollectionViewModel : INotifyCollectionChanged
    {
        void SynchronizeWithModels();
        void CompleteChanges();
    }
}
