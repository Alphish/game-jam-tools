using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public class MutableProperty<TValue> : NotifiableProperty
    {
        private TValue InnerValue { get; set; }
        private ICollection<INotifiableProperty> DependingProperties { get; }

        public MutableProperty(IViewModel viewModel, string propertyName, TValue initialValue)
            : base(viewModel, propertyName)
        {
            InnerValue = initialValue;
            DependingProperties = new List<INotifiableProperty>();
        }

        public TValue Value
        {
            get => InnerValue;
            set
            {
                if (object.Equals(InnerValue, value))
                    return;

                InnerValue = value;

                RaisePropertyChanged();
                foreach (var dependingProperty in DependingProperties)
                {
                    dependingProperty.RaisePropertyChanged();
                }
            }
        }

        // -------------------------------
        // Populating depending properties
        // -------------------------------

        public void AddDependingProperty(INotifiableProperty property)
        {
            DependingProperties.Add(property);
        }

        public void AddDependingProperty(string propertyName)
            => AddDependingProperty(this.ViewModel, propertyName);

        public void AddDependingProperty(IViewModel viewModel, string propertyName)
            => AddDependingProperty(NotifiableProperty.Create(viewModel, propertyName));

        public MutableProperty<TValue> WithDependingProperty(INotifiableProperty property)
        {
            AddDependingProperty(property);
            return this;
        }

        public MutableProperty<TValue> WithDependingProperty(IViewModel viewModel, string propertyName)
            => WithDependingProperty(NotifiableProperty.Create(viewModel, propertyName));

        public MutableProperty<TValue> WithDependingProperty(string propertyName)
            => WithDependingProperty(this.ViewModel, propertyName);
    }

    // ---------------
    // Static creation
    // ---------------

    public static class MutableProperty
    {
        public static MutableProperty<TValue> Create<TValue>(IViewModel viewModel, string propertyName, TValue initialValue)
        {
            return new MutableProperty<TValue>(viewModel, propertyName, initialValue);
        }
    }
}
