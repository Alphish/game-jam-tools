using System.Text.Json;

namespace Alphicsh.JamPlayer.Model.Ratings.NumericScale
{
    public sealed class NumericScaleRating : BaseRating<double?, NumericScaleCriterion>
    {
        public override bool HasValue => Value.HasValue;

        protected override double? FromObjectValue(object? value)
        {
            if (value is double doubleValue)
                return doubleValue;

            if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
                return jsonElement.GetDouble();

            return null;
        }
    }
}
