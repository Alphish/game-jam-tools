﻿using System.Collections.Generic;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public class NotifiableProperty : INotifiableProperty
    {
        protected IViewModel ViewModel { get; }
        protected string PropertyName { get; }
        protected ICollection<INotifiableProperty> DependingProperties { get; }

        public NotifiableProperty(IViewModel viewModel, string propertyName)
        {
            ViewModel = viewModel;
            PropertyName = propertyName;
            DependingProperties = new List<INotifiableProperty>();
        }

        public static NotifiableProperty Create(IViewModel viewModel, string propertyName)
        {
            return new NotifiableProperty(viewModel, propertyName);
        }

        public void RaisePropertyChanged()
        {
            ViewModel.RaisePropertyChanged(PropertyName);
            foreach (var dependingProperty in DependingProperties)
            {
                dependingProperty.RaisePropertyChanged();
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
    }
}
