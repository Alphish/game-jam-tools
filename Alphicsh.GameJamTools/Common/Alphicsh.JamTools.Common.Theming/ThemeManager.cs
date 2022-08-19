using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// NOTE: To extract variable names, use the following regex on ThemeVariables dictionary:
// /x:Key="(\w+)"/g
// 
// Apply the following substitution for variable definitions in the constructor:
// $1Variable = VariableFor(x => x.$1);\n
// 
// Apply the following substitution for properties declarations:
// public Brush $1 { get => $1Variable.Value; set => $1Variable.Value = value; }\n
// private ThemeVariable<Brush> $1Variable { get; }\n\n
// 
// (you can use regexr.com or something for quick substitutions)

namespace Alphicsh.JamTools.Common.Theming
{
    public class ThemeManager
    {
        private ResourceDictionary InnerDictionary { get; }

        // --------
        // Creation
        // --------

        public ThemeManager(ResourceDictionary innerDictionary)
        {
            InnerDictionary = innerDictionary;

            // General theming
            BasicTextVariable = VariableFor(x => x.BasicText);
            HighlightTextVariable = VariableFor(x => x.HighlightText);
            DimTextVariable = VariableFor(x => x.DimText);
            ExtraDimTextVariable = VariableFor(x => x.ExtraDimText);
            MainBackgroundBrushVariable = VariableFor(x => x.MainBackgroundBrush);
            MenuBackgroundBrushVariable = VariableFor(x => x.MenuBackgroundBrush);
            MouseOverHighlightVariable = VariableFor(x => x.MouseOverHighlight);
            SelectionHighlightVariable = VariableFor(x => x.SelectionHighlight);

            // Buttons theming
            ButtonBackgroundBrushVariable = VariableFor(x => x.ButtonBackgroundBrush);
            ButtonHoverBrushVariable = VariableFor(x => x.ButtonHoverBrush);
            ButtonShadowBrushVariable = VariableFor(x => x.ButtonShadowBrush);
            ButtonGlowBrushVariable = VariableFor(x => x.ButtonGlowBrush);

            PrimaryButtonBackgroundBrushVariable = VariableFor(x => x.PrimaryButtonBackgroundBrush);
            PrimaryButtonHoverBrushVariable = VariableFor(x => x.PrimaryButtonHoverBrush);
            PrimaryButtonShadowBrushVariable = VariableFor(x => x.PrimaryButtonShadowBrush);
            PrimaryButtonGlowBrushVariable = VariableFor(x => x.PrimaryButtonGlowBrush);

            // Text box theming

            TextBoxBackgroundBrushVariable = VariableFor(x => x.TextBoxBackgroundBrush);
            TextBoxFocusBrushVariable = VariableFor(x => x.TextBoxFocusBrush);
            TextBoxShadowBrushVariable = VariableFor(x => x.TextBoxShadowBrush);
            TextBoxGlowBrushVariable = VariableFor(x => x.TextBoxGlowBrush);

            TextBoxScrollBrushVariable = VariableFor(x => x.TextBoxScrollBrush);
            TextBoxScrollPressedBrushVariable = VariableFor(x => x.TextBoxScrollPressedBrush);
            TextBoxScrollDisabledBrushVariable = VariableFor(x => x.TextBoxScrollDisabledBrush);

            // Section theming
            SectionHeaderBrushVariable = VariableFor(x => x.SectionHeaderBrush);
            SectionHeaderTitleBrushVariable = VariableFor(x => x.SectionHeaderTitleBrush);
            SectionBorderBrushVariable = VariableFor(x => x.SectionBorderBrush);
            SectionBackgroundBrushVariable = VariableFor(x => x.SectionBackgroundBrush);

            SimpleSectionShadowBrushVariable = VariableFor(x => x.SimpleSectionShadowBrush);
            SimpleSectionSideBrushVariable = VariableFor(x => x.SimpleSectionSideBrush);
            SimpleSectionGlowBrushVariable = VariableFor(x => x.SimpleSectionGlowBrush);

            // List theming
            ListScrollBrushVariable = VariableFor(x => x.ListScrollBrush);
            ListScrollPressedBrushVariable = VariableFor(x => x.ListScrollPressedBrush);
            ListScrollDisabledBrushVariable = VariableFor(x => x.ListScrollDisabledBrush);

            // Images
            MenuJamIconSourceVariable = ImageVariableFor(x => x.MenuJamIconSource, "menu_jam.png");
            MenuRankingIconSourceVariable = ImageVariableFor(x => x.MenuRankingIconSource, "menu_ranking.png");
            MenuAwardsIconSourceVariable = ImageVariableFor(x => x.MenuAwardsIconSource, "menu_awards.png");

            EntryPlaceholderSourceVariable = ImageVariableFor(x => x.EntryPlaceholderSource, "entry_placeholder.png");
            StarEmptySourceVariable = ImageVariableFor(x => x.StarEmptySource, "star_empty.png");
            StarFullSourceVariable = ImageVariableFor(x => x.StarFullSource, "star_full.png");
        }

        private ThemeVariable<TValue> VariableFor<TValue>(Expression<Func<ThemeManager, TValue>> propertyExpression)
        {
            var memberAccess = (MemberExpression)propertyExpression.Body;
            var propertyName = memberAccess.Member.Name;
            return new ThemeVariable<TValue>(InnerDictionary, propertyName);
        }

        private ThemeVariable<ImageSource> ImageVariableFor(Expression<Func<ThemeManager, ImageSource>> propertyExpression, string contentName)
        {
            var variable = VariableFor(propertyExpression);
            variable.Value = LoadImageSourceResource(contentName);
            return variable;
        }

        private const string ContentPrefix = "Alphicsh.JamTools.Common.Theming.Content.";
        private ImageSource LoadImageSourceResource(string contentName)
        {
            var assembly = typeof(ThemeManager).Assembly;
            var resourceName = ContentPrefix + contentName;
            using var resourceStream = assembly.GetManifestResourceStream(resourceName);

            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.StreamSource = resourceStream;
            imageSource.CacheOption = BitmapCacheOption.OnLoad;
            imageSource.EndInit();

            return imageSource;
        }

        // ---------------
        // General theming
        // ---------------

        public Brush BasicText { get => BasicTextVariable.Value; set => BasicTextVariable.Value = value; }
        private ThemeVariable<Brush> BasicTextVariable { get; }

        public Brush HighlightText { get => HighlightTextVariable.Value; set => HighlightTextVariable.Value = value; }
        private ThemeVariable<Brush> HighlightTextVariable { get; }

        public Brush DimText { get => DimTextVariable.Value; set => DimTextVariable.Value = value; }
        private ThemeVariable<Brush> DimTextVariable { get; }

        public Brush ExtraDimText { get => ExtraDimTextVariable.Value; set => ExtraDimTextVariable.Value = value; }
        private ThemeVariable<Brush> ExtraDimTextVariable { get; }

        public Brush MainBackgroundBrush { get => MainBackgroundBrushVariable.Value; set => MainBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> MainBackgroundBrushVariable { get; }

        public Brush MenuBackgroundBrush { get => MenuBackgroundBrushVariable.Value; set => MenuBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> MenuBackgroundBrushVariable { get; }

        public Brush MouseOverHighlight { get => MouseOverHighlightVariable.Value; set => MouseOverHighlightVariable.Value = value; }
        private ThemeVariable<Brush> MouseOverHighlightVariable { get; }

        public Brush SelectionHighlight { get => SelectionHighlightVariable.Value; set => SelectionHighlightVariable.Value = value; }
        private ThemeVariable<Brush> SelectionHighlightVariable { get; }

        // ---------------
        // Buttons theming
        // ---------------

        public Brush ButtonBackgroundBrush { get => ButtonBackgroundBrushVariable.Value; set => ButtonBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonBackgroundBrushVariable { get; }

        public Brush ButtonHoverBrush { get => ButtonHoverBrushVariable.Value; set => ButtonHoverBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonHoverBrushVariable { get; }

        public Brush ButtonShadowBrush { get => ButtonShadowBrushVariable.Value; set => ButtonShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonShadowBrushVariable { get; }

        public Brush ButtonGlowBrush { get => ButtonGlowBrushVariable.Value; set => ButtonGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonGlowBrushVariable { get; }

        public Brush PrimaryButtonBackgroundBrush { get => PrimaryButtonBackgroundBrushVariable.Value; set => PrimaryButtonBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonBackgroundBrushVariable { get; }

        public Brush PrimaryButtonHoverBrush { get => PrimaryButtonHoverBrushVariable.Value; set => PrimaryButtonHoverBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonHoverBrushVariable { get; }

        public Brush PrimaryButtonShadowBrush { get => PrimaryButtonShadowBrushVariable.Value; set => PrimaryButtonShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonShadowBrushVariable { get; }

        public Brush PrimaryButtonGlowBrush { get => PrimaryButtonGlowBrushVariable.Value; set => PrimaryButtonGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonGlowBrushVariable { get; }

        // ----------------
        // Text box theming
        // ----------------

        public Brush TextBoxBackgroundBrush { get => TextBoxBackgroundBrushVariable.Value; set => TextBoxBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxBackgroundBrushVariable { get; }

        public Brush TextBoxFocusBrush { get => TextBoxFocusBrushVariable.Value; set => TextBoxFocusBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxFocusBrushVariable { get; }

        public Brush TextBoxShadowBrush { get => TextBoxShadowBrushVariable.Value; set => TextBoxShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxShadowBrushVariable { get; }

        public Brush TextBoxGlowBrush { get => TextBoxGlowBrushVariable.Value; set => TextBoxGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxGlowBrushVariable { get; }

        public Brush TextBoxScrollBrush { get => TextBoxScrollBrushVariable.Value; set => TextBoxScrollBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxScrollBrushVariable { get; }

        public Brush TextBoxScrollPressedBrush { get => TextBoxScrollPressedBrushVariable.Value; set => TextBoxScrollPressedBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxScrollPressedBrushVariable { get; }

        public Brush TextBoxScrollDisabledBrush { get => TextBoxScrollDisabledBrushVariable.Value; set => TextBoxScrollDisabledBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxScrollDisabledBrushVariable { get; }

        // ---------------
        // Section theming
        // ---------------

        public Brush SectionHeaderBrush { get => SectionHeaderBrushVariable.Value; set => SectionHeaderBrushVariable.Value = value; }
        private ThemeVariable<Brush> SectionHeaderBrushVariable { get; }

        public Brush SectionHeaderTitleBrush { get => SectionHeaderTitleBrushVariable.Value; set => SectionHeaderTitleBrushVariable.Value = value; }
        private ThemeVariable<Brush> SectionHeaderTitleBrushVariable { get; }

        public Brush SectionBorderBrush { get => SectionBorderBrushVariable.Value; set => SectionBorderBrushVariable.Value = value; }
        private ThemeVariable<Brush> SectionBorderBrushVariable { get; }

        public Brush SectionBackgroundBrush { get => SectionBackgroundBrushVariable.Value; set => SectionBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> SectionBackgroundBrushVariable { get; }

        public Brush SimpleSectionShadowBrush { get => SimpleSectionShadowBrushVariable.Value; set => SimpleSectionShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> SimpleSectionShadowBrushVariable { get; }

        public Brush SimpleSectionSideBrush { get => SimpleSectionSideBrushVariable.Value; set => SimpleSectionSideBrushVariable.Value = value; }
        private ThemeVariable<Brush> SimpleSectionSideBrushVariable { get; }

        public Brush SimpleSectionGlowBrush { get => SimpleSectionGlowBrushVariable.Value; set => SimpleSectionGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> SimpleSectionGlowBrushVariable { get; }

        // ------------
        // List theming
        // ------------

        public Brush ListScrollBrush { get => ListScrollBrushVariable.Value; set => ListScrollBrushVariable.Value = value; }
        private ThemeVariable<Brush> ListScrollBrushVariable { get; }

        public Brush ListScrollPressedBrush { get => ListScrollPressedBrushVariable.Value; set => ListScrollPressedBrushVariable.Value = value; }
        private ThemeVariable<Brush> ListScrollPressedBrushVariable { get; }

        public Brush ListScrollDisabledBrush { get => ListScrollDisabledBrushVariable.Value; set => ListScrollDisabledBrushVariable.Value = value; }
        private ThemeVariable<Brush> ListScrollDisabledBrushVariable { get; }

        // ------
        // Images
        // ------

        public ImageSource MenuJamIconSource { get => MenuJamIconSourceVariable.Value; set => MenuJamIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuJamIconSourceVariable { get; }

        public ImageSource MenuRankingIconSource { get => MenuRankingIconSourceVariable.Value; set => MenuRankingIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuRankingIconSourceVariable { get; }

        public ImageSource MenuAwardsIconSource { get => MenuAwardsIconSourceVariable.Value; set => MenuAwardsIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuAwardsIconSourceVariable { get; }

        public ImageSource EntryPlaceholderSource { get => EntryPlaceholderSourceVariable.Value; set => EntryPlaceholderSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> EntryPlaceholderSourceVariable { get; }

        public ImageSource StarEmptySource { get => StarEmptySourceVariable.Value; set => StarEmptySourceVariable.Value = value; }
        private ThemeVariable<ImageSource> StarEmptySourceVariable { get; }

        public ImageSource StarFullSource { get => StarFullSourceVariable.Value; set => StarFullSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> StarFullSourceVariable { get; }
    }
}
