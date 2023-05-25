using System;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public interface INotifiableProperty
    {
        void RaisePropertyChanged();
        event EventHandler? PropertyChanged;
    }
}
