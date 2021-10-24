using System.ComponentModel;

namespace Alphicsh.JamPlayer.ViewModel
{
    public interface IViewModel : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string propertyName);
    }
}
