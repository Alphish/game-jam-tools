namespace Alphicsh.JamPlayer.Model.Ratings.NumericScale
{
    public interface INumericScaleSkin : IRatingSkin
    {
        string? BackgroundKey { get; }
        string? BackgroundImageKey { get; }
        string? BackgroundMaskKey { get; }
        string? BackgroundMaskImageKey { get; }

        string? ForegroundKey { get; }
        string? ForegroundImageKey { get; }
        string? ForegroundMaskKey { get; }
        string? ForegroundMaskImageKey { get; }

        string? NoValueBackgroundKey { get; }
        string? NoValueBackgroundImageKey { get; }
    }
}
