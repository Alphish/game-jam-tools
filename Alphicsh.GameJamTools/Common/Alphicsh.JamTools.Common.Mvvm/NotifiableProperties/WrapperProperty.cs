using System;

namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public class WrapperProperty<TViewModel, TValue> : NotifiableProperty
        where TViewModel : IViewModel
    {
        protected new TViewModel ViewModel => (TViewModel)base.ViewModel;

        private Func<TViewModel, TValue> ValueGetter { get; set; }
        private Action<TViewModel, TValue>? ValueSetter { get; set; }

        public bool IsReadonly => ValueSetter == null;
        public TValue Value
        {
            get => ValueGetter(ViewModel);
            set
            {
                if (ValueSetter == null)
                    throw new NotSupportedException("Cannot set a value of the readonly wrapper property.");

                ValueSetter.Invoke(ViewModel, value);
            }
        }

        public WrapperProperty(
            TViewModel viewModel, string propertyName, Func<TViewModel, TValue> valueGetter, Action<TViewModel, TValue>? valueSetter
            ) : base(viewModel, propertyName)
        {
            ValueGetter = valueGetter;
            ValueSetter = valueSetter;
        }
    }

    // ---------------
    // Static creation
    // ---------------

    public static class WrapperProperty
    {
        public static WrapperProperty<TViewModel, TValue> Create<TViewModel, TValue>(
            TViewModel viewModel, string propertyName, Func<TViewModel, TValue> valueGetter, Action<TViewModel, TValue> valueSetter
            )
            where TViewModel : IViewModel
        {
            return new WrapperProperty<TViewModel, TValue>(viewModel, propertyName, valueGetter, valueSetter);
        }

        public static WrapperProperty<TViewModel, TValue> CreateReadonly<TViewModel, TValue>(
            TViewModel viewModel, string propertyName, Func<TViewModel, TValue> valueGetter
            )
            where TViewModel : IViewModel
        {
            return new WrapperProperty<TViewModel, TValue>(viewModel, propertyName, valueGetter, valueSetter: null);
        }
    }
}
