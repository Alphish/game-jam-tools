using System.ComponentModel;

namespace Alphicsh.JamPlayer.ViewModel
{
    public abstract class BaseViewModel<TModel> : IViewModel
    {
        protected internal TModel Model { get; }

        protected BaseViewModel(TModel model)
        {
            Model = model;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
