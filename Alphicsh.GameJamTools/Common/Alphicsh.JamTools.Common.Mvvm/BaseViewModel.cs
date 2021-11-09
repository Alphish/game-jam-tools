using System.ComponentModel;

namespace Alphicsh.JamTools.Common.Mvvm
{
    public abstract class BaseViewModel : IViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
