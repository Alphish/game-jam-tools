﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alphicsh.JamTools.Common.Mvvm;

using Alphicsh.JamPlayer.Model.Ratings;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;
using System.Windows.Input;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using System.Windows.Media;
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
                .WithDependingProperty(nameof(HasValue));

            ClearValueCommand = new SimpleCommand(() => ValueProperty.Value = null);
        }

        public double MaxValue => Model.Options.MaxValue;
        public double ValueStep => Model.Options.ValueStep;
        public INumericScaleSkin Skin => Model.Options.Skin;

        public WrapperProperty<NumericScaleRatingViewModel, double?> ValueProperty { get; }
        public ICommand ClearValueCommand { get; }
        public double? Value { get => ValueProperty.Value; set => ValueProperty.Value = value; }
        public double DisplayValue { get => ValueProperty.Value ?? 0d; set => ValueProperty.Value = value; }
        public bool HasValue => Model.HasValue;
    }
}