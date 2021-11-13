using System.Windows;

using Alphicsh.JamTools.Common.Controls;

using Alphicsh.JamPlayer.Model.Ratings.NumericScale;

namespace Alphicsh.JamPlayer.Controls.Ratings
{
    public static class NumericScaleTheming
    {
        public static readonly DependencyProperty SkinProperty = DependencyProperty.RegisterAttached(
            "Skin", typeof(INumericScaleSkin), typeof(NumericScaleTheming), new PropertyMetadata(defaultValue: null, OnSkinPropertyChange)
            );

        public static INumericScaleSkin GetSkin(DependencyObject obj)
            => (INumericScaleSkin)obj.GetValue(SkinProperty);
        public static void SetSkin(DependencyObject obj, INumericScaleSkin value)
            => obj.SetValue(SkinProperty, value);

        private static void OnSkinPropertyChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as RatingSlider;
            var skin = e.NewValue as INumericScaleSkin;
            if (control == null || skin == null)
                return;

            ApplyResource(control, RatingSlider.BackgroundProperty, skin.BackgroundKey);
            ApplyResource(control, RatingSlider.BackgroundImageProperty, skin.BackgroundImageKey);
            ApplyResource(control, RatingSlider.BackgroundMaskProperty, skin.BackgroundMaskKey);
            ApplyResource(control, RatingSlider.BackgroundMaskImageProperty, skin.BackgroundMaskImageKey);

            ApplyResource(control, RatingSlider.ForegroundProperty, skin.ForegroundKey);
            ApplyResource(control, RatingSlider.ForegroundImageProperty, skin.ForegroundImageKey);
            ApplyResource(control, RatingSlider.ForegroundMaskProperty, skin.ForegroundMaskKey);
            ApplyResource(control, RatingSlider.ForegroundMaskImageProperty, skin.ForegroundMaskImageKey);

            ApplyResource(control, RatingSlider.NoValueBackgroundProperty, skin.NoValueBackgroundKey);
            ApplyResource(control, RatingSlider.NoValueBackgroundImageProperty, skin.NoValueBackgroundImageKey);
        }

        private static void ApplyResource(RatingSlider control, DependencyProperty property, string? resourceKey)
        {
            if (resourceKey == null)
                control.ClearValue(property);
            else
                control.SetResourceReference(property, resourceKey);
        }
    }
}
