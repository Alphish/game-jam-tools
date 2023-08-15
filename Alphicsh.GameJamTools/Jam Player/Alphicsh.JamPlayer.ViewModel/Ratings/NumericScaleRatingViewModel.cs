using System.Windows.Input;

using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

using Alphicsh.JamPlayer.Model.Ratings.NumericScale;

namespace Alphicsh.JamPlayer.ViewModel.Ratings
{
    public class NumericScaleRatingViewModel : RatingViewModel
    {
        private new NumericScaleRating Model
            => (NumericScaleRating)base.Model;

        public NumericScaleRatingViewModel(NumericScaleRating model)
            : base(model)
        {
            ValueProperty = WrapperProperty.Create(this, nameof(Value), vm => vm.Model.Value, (vm, value) => vm.Model.Value = value)
                .WithDependingProperty(nameof(DisplayValue))
                .WithDependingProperty(nameof(HasValue))
                .WithDependingProperty(GenericValueProperty);

            ClearValueCommand = SimpleCommand.From(() => ValueProperty.Value = null);
            ZeroValueCommand = SimpleCommand.From(() => ValueProperty.Value = 0);
        }

        public double MaxValue => Model.Criterion.MaxValue;
        public double ValueStep => Model.Criterion.ValueStep;
        public INumericScaleSkin Skin => Model.Criterion.Skin;

        public WrapperProperty<NumericScaleRatingViewModel, double?> ValueProperty { get; }
        public ICommand ClearValueCommand { get; }
        public ICommand ZeroValueCommand { get; }
        public double? Value { get => ValueProperty.Value; set => ValueProperty.Value = value; }
        public double DisplayValue { get => ValueProperty.Value ?? 0d; set => ValueProperty.Value = value; }
        public bool HasValue => Model.HasValue;

        public override object GenericValue => Value ?? -1;
    }
}
