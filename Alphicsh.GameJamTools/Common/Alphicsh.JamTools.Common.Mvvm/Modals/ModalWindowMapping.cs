using System;
using System.Collections.Generic;
using System.Windows;

namespace Alphicsh.JamTools.Common.Mvvm.Modals
{
    public static class ModalWindowMapping
    {
        private static Dictionary<Type, Type> ViewModelWindows { get; } = new Dictionary<Type, Type>();

        public static void Add<TViewModel, TWindow>()
            where TViewModel : ModalViewModel
            where TWindow : Window, new()
        {
            var viewModelType = typeof(TViewModel);
            var windowType = typeof(TWindow);
            if (ViewModelWindows.ContainsKey(typeof(TViewModel)))
            {
                var message = $"The view model type '{viewModelType.Name}' already maps to window type '{windowType.Name}'.";
                throw new InvalidOperationException(message);
            }

            ViewModelWindows.Add(typeof(TViewModel), typeof(TWindow));
        }

        internal static Window CreateWindowFor(Type viewModelType)
        {
            if (!ViewModelWindows.ContainsKey(viewModelType))
                throw new InvalidOperationException($"The view model type '{viewModelType.Name}' has no mapping to window type.");

            var windowType = ViewModelWindows[viewModelType];
            var window = Activator.CreateInstance(windowType);
            return (Window)window!;
        }
    }
}
