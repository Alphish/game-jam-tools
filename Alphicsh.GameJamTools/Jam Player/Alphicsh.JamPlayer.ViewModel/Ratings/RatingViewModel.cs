using System;

using Alphicsh.JamPlayer.Model.Ratings;
using Alphicsh.JamPlayer.Model.Ratings.NumericScale;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPlayer.ViewModel.Ratings
{
    public abstract class RatingViewModel : WrapperViewModel<IRating>
    {
        protected RatingViewModel(IRating model)
            : base(model)
        {
            GenericValueProperty = NotifiableProperty.Create(this, nameof(GenericValue));
        }

        public static RatingViewModel Create(IRating rating)
        {
            switch (rating)
            {
                case NumericScaleRating numericScaleRating:
                    return new NumericScaleRatingViewModel(numericScaleRating);
                default:
                    throw new NotSupportedException($"The rating type {rating.GetType().Name} cannot be converted to view model.");
            }
        }

        public string Id => Model.Id;
        public string Name => Model.Name;

        public abstract object GenericValue { get; }
        public NotifiableProperty GenericValueProperty { get; }
    }
}
