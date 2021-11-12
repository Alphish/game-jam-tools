namespace Alphicsh.JamPlayer.Model.Ratings.NumericScale
{
    public class NumericScaleMaskSkin : INumericScaleSkin
    {
        public string Id { get; set; } = default!;

        public string ActiveBrushKey { get; set; } = default!;
        public string NoValueBrushKey { get; set; } = default!;
        public string BackgroundMaskKey { get; set; } = default!;
        public string ForegroundMaskKey { get; set; } = default!;

        string? INumericScaleSkin.BackgroundKey => this.ActiveBrushKey;
        string? INumericScaleSkin.BackgroundImageKey => null;
        string? INumericScaleSkin.BackgroundMaskKey => null;
        string? INumericScaleSkin.BackgroundMaskImageKey => this.BackgroundMaskKey;

        string? INumericScaleSkin.ForegroundKey => this.ActiveBrushKey;
        string? INumericScaleSkin.ForegroundImageKey => null;
        string? INumericScaleSkin.ForegroundMaskKey => null;
        string? INumericScaleSkin.ForegroundMaskImageKey => this.ForegroundMaskKey;

        string? INumericScaleSkin.NoValueBackgroundKey => this.NoValueBrushKey;
        string? INumericScaleSkin.NoValueBackgroundImageKey => null;

    }
}
