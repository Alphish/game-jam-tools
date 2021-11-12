namespace Alphicsh.JamTools.Common.Mvvm.NotifiableProperties
{
    public class MutableProperty<TValue> : NotifiableProperty
    {
        private TValue InnerValue { get; set; }

        public MutableProperty(IViewModel viewModel, string propertyName, TValue initialValue)
            : base(viewModel, propertyName)
        {
            InnerValue = initialValue;
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
            }
        }
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
