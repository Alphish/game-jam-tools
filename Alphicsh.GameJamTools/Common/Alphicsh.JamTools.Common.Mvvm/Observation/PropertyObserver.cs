using System;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTools.Common.Mvvm.Observation
{
    public class PropertyObserver : IObserverNode
    {
        private TreeObserver MainObserver { get; }
        private INotifiableProperty Property { get; }

        public PropertyObserver(TreeObserver mainObserver, INotifiableProperty property)
        {
            MainObserver = mainObserver;
            Property = property;
        }

        public void Observe()
        {
            Property.PropertyChanged += OnPropertyChanged;
        }

        public void Unobserve()
        {
            Property.PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, EventArgs e)
        {
            MainObserver.RecordObservation();
        }
    }
}
