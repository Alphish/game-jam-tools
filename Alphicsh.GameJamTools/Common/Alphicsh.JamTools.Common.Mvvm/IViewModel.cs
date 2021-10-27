using System.ComponentModel;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public interface IViewModel : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string propertyName);
    }
}
