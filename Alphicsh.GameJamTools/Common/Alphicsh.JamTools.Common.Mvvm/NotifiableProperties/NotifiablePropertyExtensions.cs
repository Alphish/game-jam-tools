using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public static class NotifiablePropertyExtensions
    {
        public static TProperty WithDependingProperty<TProperty>(this TProperty property, INotifiableProperty dependingProperty)
            where TProperty : NotifiableProperty
        {
            property.AddDependingProperty(dependingProperty);
            return property;
        }

        public static TProperty WithDependingProperty<TProperty>(this TProperty property, IViewModel viewModel, string propertyName)
            where TProperty : NotifiableProperty
        {
            property.AddDependingProperty(viewModel, propertyName);
            return property;
        }

        public static TProperty WithDependingProperty<TProperty>(this TProperty property, string propertyName)
            where TProperty : NotifiableProperty
        {
            property.AddDependingProperty(propertyName);
            return property;
        }

        public static TProperty WithDependingCollection<TProperty>(this TProperty property, ICollectionViewModel collection)
            where TProperty : NotifiableProperty
        {
            property.AddDependingCollection(collection);
            return property;
        }

        public static TProperty WithDependingCommand<TProperty>(this TProperty property, IConditionalCommand command)
            where TProperty : NotifiableProperty
        {
            property.AddDependingCommand(command);
            return property;
        }
    }
}
