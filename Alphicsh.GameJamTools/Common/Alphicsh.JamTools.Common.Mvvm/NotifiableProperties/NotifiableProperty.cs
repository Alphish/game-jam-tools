using System.Collections.Generic;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public class NotifiableProperty : INotifiableProperty
    {
        protected IViewModel ViewModel { get; }
        protected string PropertyName { get; }
        protected ICollection<INotifiableProperty> DependingProperties { get; }
        protected ICollection<ICollectionViewModel> DependingCollections { get; }
        protected ICollection<IConditionalCommand> DependingCommands { get; }

        public NotifiableProperty(IViewModel viewModel, string propertyName)
        {
            ViewModel = viewModel;
            PropertyName = propertyName;
            DependingProperties = new List<INotifiableProperty>();
            DependingCollections = new List<ICollectionViewModel>();
            DependingCommands = new List<IConditionalCommand>();
        }

        public static NotifiableProperty Create(IViewModel viewModel, string propertyName)
        {
            return new NotifiableProperty(viewModel, propertyName);
        }

        public virtual void OnPropertyChange()
        {
            // nothing by default
        }

        public void RaisePropertyChanged()
        {
            OnPropertyChange();
            ViewModel.RaisePropertyChanged(PropertyName);

            foreach (var dependingProperty in DependingProperties)
            {
                dependingProperty.RaisePropertyChanged();
            }
            foreach (var dependingCollection in DependingCollections)
            {
                dependingCollection.SynchronizeWithModels();
            }
            foreach (var dependingCommand in DependingCommands)
            {
                dependingCommand.RaiseCanExecuteChanged();
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

        public void AddDependingCollection(ICollectionViewModel collection)
        {
            DependingCollections.Add(collection);
        }

        public void AddDependingCommand(IConditionalCommand command)
        {
            DependingCommands.Add(command);
        }
    }
}
