﻿using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public static class NotifiablePropertyExtensions
    {
        // ----------------------
        // Adding depending items
        // ----------------------

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

        // -------------------
        // Adding dependencies
        // -------------------

        public static TProperty DependingOn<TProperty>(this TProperty property, NotifiableProperty dependencyProperty)
            where TProperty : INotifiableProperty
        {
            dependencyProperty.AddDependingProperty(property);
            return property;
        }

        public static TProperty DependingOn<TProperty>(this TProperty property, params NotifiableProperty[] dependencyProperties)
            where TProperty : INotifiableProperty
        {
            foreach (var dependencyProperty in dependencyProperties)
            {
                dependencyProperty.AddDependingProperty(property);
            }
            return property;
        }

        public static TCollection ItemsDependingOn<TCollection>(this TCollection collection, NotifiableProperty dependencyProperty)
            where TCollection : ICollectionViewModel
        {
            dependencyProperty.AddDependingCollection(collection);
            return collection;
        }

        public static TCollection ItemsDependingOn<TCollection>(this TCollection collection, params NotifiableProperty[] dependencyProperties)
            where TCollection : ICollectionViewModel
        {
            foreach (var dependencyProperty in dependencyProperties)
            {
                dependencyProperty.AddDependingCollection(collection);
            }
            return collection;
        }

        public static TCommand ExecutionDependingOn<TCommand>(this TCommand command, NotifiableProperty dependencyProperty)
            where TCommand : IConditionalCommand
        {
            dependencyProperty.AddDependingCommand(command);
            return command;
        }

        public static TCommand ExecutionDependingOn<TCommand>(this TCommand command, params NotifiableProperty[] dependencyProperties)
            where TCommand : IConditionalCommand
        {
            foreach (var dependencyProperty in dependencyProperties)
            {
                dependencyProperty.AddDependingCommand(command);
            }
            return command;
        }
    }
}
