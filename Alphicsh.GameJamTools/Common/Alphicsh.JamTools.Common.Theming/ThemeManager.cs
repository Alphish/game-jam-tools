﻿using System;
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

        public static ThemeManager Current { get; private set; } = default!;

        public static ThemeManager Create(ResourceDictionary innerDictionary)
        {
            if (Current != null)
                throw new InvalidOperationException("There can be only one ThemeManager instance at a time.");

            var manager = new ThemeManager(innerDictionary);
            Current = manager;
            return manager;
        }

        public ThemeManager(ResourceDictionary innerDictionary)
        {
            InnerDictionary = innerDictionary;

            // General theming
            BasicTextVariable = VariableFor(x => x.BasicText);
            HighlightTextVariable = VariableFor(x => x.HighlightText);
            DimTextVariable = VariableFor(x => x.DimText);
            ExtraDimTextVariable = VariableFor(x => x.ExtraDimText);
            DisabledTextVariable = VariableFor(x => x.DisabledText);
            ErrorTextVariable = VariableFor(x => x.ErrorText);

            MainBackgroundBrushVariable = VariableFor(x => x.MainBackgroundBrush);
            MenuBackgroundBrushVariable = VariableFor(x => x.MenuBackgroundBrush);
            MouseOverHighlightVariable = VariableFor(x => x.MouseOverHighlight);
            SelectionHighlightVariable = VariableFor(x => x.SelectionHighlight);

            // Buttons theming
            ButtonBackgroundBrushVariable = VariableFor(x => x.ButtonBackgroundBrush);
            ButtonHoverBrushVariable = VariableFor(x => x.ButtonHoverBrush);
            ButtonShadowBrushVariable = VariableFor(x => x.ButtonShadowBrush);
            ButtonGlowBrushVariable = VariableFor(x => x.ButtonGlowBrush);
            ButtonDisabledForegroundBrushVariable = VariableFor(x => x.ButtonDisabledForegroundBrush);
            ButtonDisabledBackgroundBrushVariable = VariableFor(x => x.ButtonDisabledBackgroundBrush);
            ButtonDisabledShadowBrushVariable = VariableFor(x => x.ButtonDisabledShadowBrush);
            ButtonDisabledGlowBrushVariable = VariableFor(x => x.ButtonDisabledGlowBrush);

            PrimaryButtonBackgroundBrushVariable = VariableFor(x => x.PrimaryButtonBackgroundBrush);
            PrimaryButtonHoverBrushVariable = VariableFor(x => x.PrimaryButtonHoverBrush);
            PrimaryButtonShadowBrushVariable = VariableFor(x => x.PrimaryButtonShadowBrush);
            PrimaryButtonGlowBrushVariable = VariableFor(x => x.PrimaryButtonGlowBrush);
            PrimaryButtonDisabledBackgroundBrushVariable = VariableFor(x => x.PrimaryButtonDisabledBackgroundBrush);
            PrimaryButtonDisabledShadowBrushVariable = VariableFor(x => x.PrimaryButtonDisabledShadowBrush);
            PrimaryButtonDisabledGlowBrushVariable = VariableFor(x => x.PrimaryButtonDisabledGlowBrush);

            HelpButtonBackgroundBrushVariable = VariableFor(x => x.HelpButtonBackgroundBrush);
            HelpButtonHoverBrushVariable = VariableFor(x => x.HelpButtonHoverBrush);
            HelpButtonShadowBrushVariable = VariableFor(x => x.HelpButtonShadowBrush);
            HelpButtonGlowBrushVariable = VariableFor(x => x.HelpButtonGlowBrush);
            HelpButtonDisabledBackgroundBrushVariable = VariableFor(x => x.HelpButtonDisabledBackgroundBrush);
            HelpButtonDisabledShadowBrushVariable = VariableFor(x => x.HelpButtonDisabledShadowBrush);
            HelpButtonDisabledGlowBrushVariable = VariableFor(x => x.HelpButtonDisabledGlowBrush);

            DangerButtonBackgroundBrushVariable = VariableFor(x => x.DangerButtonBackgroundBrush);
            DangerButtonHoverBrushVariable = VariableFor(x => x.DangerButtonHoverBrush);
            DangerButtonShadowBrushVariable = VariableFor(x => x.DangerButtonShadowBrush);
            DangerButtonGlowBrushVariable = VariableFor(x => x.DangerButtonGlowBrush);
            DangerButtonDisabledBackgroundBrushVariable = VariableFor(x => x.DangerButtonDisabledBackgroundBrush);
            DangerButtonDisabledShadowBrushVariable = VariableFor(x => x.DangerButtonDisabledShadowBrush);
            DangerButtonDisabledGlowBrushVariable = VariableFor(x => x.DangerButtonDisabledGlowBrush);

            // Text box theming
            TextBoxBackgroundBrushVariable = VariableFor(x => x.TextBoxBackgroundBrush);
            TextBoxFocusBrushVariable = VariableFor(x => x.TextBoxFocusBrush);
            TextBoxShadowBrushVariable = VariableFor(x => x.TextBoxShadowBrush);
            TextBoxGlowBrushVariable = VariableFor(x => x.TextBoxGlowBrush);

            TextBoxScrollBrushVariable = VariableFor(x => x.TextBoxScrollBrush);
            TextBoxScrollPressedBrushVariable = VariableFor(x => x.TextBoxScrollPressedBrush);
            TextBoxScrollDisabledBrushVariable = VariableFor(x => x.TextBoxScrollDisabledBrush);

            TextBoxErrorOverlayBrushVariable = VariableFor(x => x.TextBoxErrorOverlayBrush);

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

            // Separator theming
            SeparatorBrushVariable = VariableFor(x => x.SeparatorBrush);

            // Modal theming
            ModalHeaderBrushVariable = VariableFor(x => x.ModalHeaderBrush);
            ModalCaptionBrushVariable = VariableFor(x => x.ModalCaptionBrush);
            ModalBodyBrushVariable = VariableFor(x => x.ModalBodyBrush);
            ModalToolbarBrushVariable = VariableFor(x => x.ModalToolbarBrush);
            ModalBorderBrushVariable = VariableFor(x => x.ModalBorderBrush);

            // Images
            MenuJamIconSourceVariable = ImageVariableFor(x => x.MenuJamIconSource, "menu_jam.png");
            MenuRankingIconSourceVariable = ImageVariableFor(x => x.MenuRankingIconSource, "menu_ranking.png");
            MenuAwardsIconSourceVariable = ImageVariableFor(x => x.MenuAwardsIconSource, "menu_awards.png");
            MenuExportIconSourceVariable = ImageVariableFor(x => x.MenuExportIconSource, "menu_export.png");
            MenuPreviewIconSourceVariable = ImageVariableFor(x => x.MenuPreviewIconSource, "menu_preview.png");
            MenuSummaryIconSourceVariable = ImageVariableFor(x => x.MenuSummaryIconSource, "menu_summary.png");
            MenuFilesIconSourceVariable = ImageVariableFor(x => x.MenuFilesIconSource, "menu_files.png");
            MenuJampackIconSourceVariable = ImageVariableFor(x => x.MenuJampackIconSource, "menu_jampack.png");

            MenuSaveIconSourceVariable = ImageVariableFor(x => x.MenuSaveIconSource, "menu_save.png");
            MenuSaveModifiedIconSourceVariable = ImageVariableFor(x => x.MenuSaveModifiedIconSource, "menu_save_modified.png");

            IconBinSourceVariable = ImageVariableFor(x => x.IconBinSource, "icon_bin.png");
            IconCenterSourceVariable = ImageVariableFor(x => x.IconCenterSource, "icon_center.png");
            IconCheckSourceVariable = ImageVariableFor(x => x.IconCheckSource, "icon_check.png");
            IconCrossSourceVariable = ImageVariableFor(x => x.IconCrossSource, "icon_cross.png");
            IconNilSourceVariable = ImageVariableFor(x => x.IconNilSource, "icon_nil.png");
            IconPreviewSourceVariable = ImageVariableFor(x => x.IconPreviewSource, "icon_preview.png");
            IconSaveSourceVariable = ImageVariableFor(x => x.IconSaveSource, "icon_save.png");

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

        public Brush DisabledText { get => DisabledTextVariable.Value; set => DisabledTextVariable.Value = value; }
        private ThemeVariable<Brush> DisabledTextVariable { get; }

        public Brush ErrorText { get => ErrorTextVariable.Value; set => ErrorTextVariable.Value = value; }
        private ThemeVariable<Brush> ErrorTextVariable { get; }

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

        public Brush ButtonDisabledForegroundBrush { get => ButtonDisabledForegroundBrushVariable.Value; set => ButtonDisabledForegroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonDisabledForegroundBrushVariable { get; }

        public Brush ButtonDisabledBackgroundBrush { get => ButtonDisabledBackgroundBrushVariable.Value; set => ButtonDisabledBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonDisabledBackgroundBrushVariable { get; }

        public Brush ButtonDisabledShadowBrush { get => ButtonDisabledShadowBrushVariable.Value; set => ButtonDisabledShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonDisabledShadowBrushVariable { get; }

        public Brush ButtonDisabledGlowBrush { get => ButtonDisabledGlowBrushVariable.Value; set => ButtonDisabledGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> ButtonDisabledGlowBrushVariable { get; }

        public Brush PrimaryButtonBackgroundBrush { get => PrimaryButtonBackgroundBrushVariable.Value; set => PrimaryButtonBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonBackgroundBrushVariable { get; }

        public Brush PrimaryButtonHoverBrush { get => PrimaryButtonHoverBrushVariable.Value; set => PrimaryButtonHoverBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonHoverBrushVariable { get; }

        public Brush PrimaryButtonShadowBrush { get => PrimaryButtonShadowBrushVariable.Value; set => PrimaryButtonShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonShadowBrushVariable { get; }

        public Brush PrimaryButtonGlowBrush { get => PrimaryButtonGlowBrushVariable.Value; set => PrimaryButtonGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonGlowBrushVariable { get; }

        public Brush PrimaryButtonDisabledBackgroundBrush { get => PrimaryButtonDisabledBackgroundBrushVariable.Value; set => PrimaryButtonDisabledBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonDisabledBackgroundBrushVariable { get; }

        public Brush PrimaryButtonDisabledShadowBrush { get => PrimaryButtonDisabledShadowBrushVariable.Value; set => PrimaryButtonDisabledShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonDisabledShadowBrushVariable { get; }

        public Brush PrimaryButtonDisabledGlowBrush { get => PrimaryButtonDisabledGlowBrushVariable.Value; set => PrimaryButtonDisabledGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> PrimaryButtonDisabledGlowBrushVariable { get; }

        public Brush HelpButtonBackgroundBrush { get => HelpButtonBackgroundBrushVariable.Value; set => HelpButtonBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> HelpButtonBackgroundBrushVariable { get; }

        public Brush HelpButtonHoverBrush { get => HelpButtonHoverBrushVariable.Value; set => HelpButtonHoverBrushVariable.Value = value; }
        private ThemeVariable<Brush> HelpButtonHoverBrushVariable { get; }

        public Brush HelpButtonShadowBrush { get => HelpButtonShadowBrushVariable.Value; set => HelpButtonShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> HelpButtonShadowBrushVariable { get; }

        public Brush HelpButtonGlowBrush { get => HelpButtonGlowBrushVariable.Value; set => HelpButtonGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> HelpButtonGlowBrushVariable { get; }

        public Brush HelpButtonDisabledBackgroundBrush { get => HelpButtonDisabledBackgroundBrushVariable.Value; set => HelpButtonDisabledBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> HelpButtonDisabledBackgroundBrushVariable { get; }

        public Brush HelpButtonDisabledShadowBrush { get => HelpButtonDisabledShadowBrushVariable.Value; set => HelpButtonDisabledShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> HelpButtonDisabledShadowBrushVariable { get; }

        public Brush HelpButtonDisabledGlowBrush { get => HelpButtonDisabledGlowBrushVariable.Value; set => HelpButtonDisabledGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> HelpButtonDisabledGlowBrushVariable { get; }

        public Brush DangerButtonBackgroundBrush { get => DangerButtonBackgroundBrushVariable.Value; set => DangerButtonBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> DangerButtonBackgroundBrushVariable { get; }

        public Brush DangerButtonHoverBrush { get => DangerButtonHoverBrushVariable.Value; set => DangerButtonHoverBrushVariable.Value = value; }
        private ThemeVariable<Brush> DangerButtonHoverBrushVariable { get; }

        public Brush DangerButtonShadowBrush { get => DangerButtonShadowBrushVariable.Value; set => DangerButtonShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> DangerButtonShadowBrushVariable { get; }

        public Brush DangerButtonGlowBrush { get => DangerButtonGlowBrushVariable.Value; set => DangerButtonGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> DangerButtonGlowBrushVariable { get; }

        public Brush DangerButtonDisabledBackgroundBrush { get => DangerButtonDisabledBackgroundBrushVariable.Value; set => DangerButtonDisabledBackgroundBrushVariable.Value = value; }
        private ThemeVariable<Brush> DangerButtonDisabledBackgroundBrushVariable { get; }

        public Brush DangerButtonDisabledShadowBrush { get => DangerButtonDisabledShadowBrushVariable.Value; set => DangerButtonDisabledShadowBrushVariable.Value = value; }
        private ThemeVariable<Brush> DangerButtonDisabledShadowBrushVariable { get; }

        public Brush DangerButtonDisabledGlowBrush { get => DangerButtonDisabledGlowBrushVariable.Value; set => DangerButtonDisabledGlowBrushVariable.Value = value; }
        private ThemeVariable<Brush> DangerButtonDisabledGlowBrushVariable { get; }

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

        public Brush TextBoxErrorOverlayBrush { get => TextBoxErrorOverlayBrushVariable.Value; set => TextBoxErrorOverlayBrushVariable.Value = value; }
        private ThemeVariable<Brush> TextBoxErrorOverlayBrushVariable { get; }

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

        // -----------------
        // Separator theming
        // -----------------

        public Brush SeparatorBrush { get => SeparatorBrushVariable.Value; set => SeparatorBrushVariable.Value = value; }
        private ThemeVariable<Brush> SeparatorBrushVariable { get; }

        // -------------
        // Modal theming
        // -------------

        public Brush ModalHeaderBrush { get => ModalHeaderBrushVariable.Value; set => ModalHeaderBrushVariable.Value = value; }
        private ThemeVariable<Brush> ModalHeaderBrushVariable { get; }

        public Brush ModalCaptionBrush { get => ModalCaptionBrushVariable.Value; set => ModalCaptionBrushVariable.Value = value; }
        private ThemeVariable<Brush> ModalCaptionBrushVariable { get; }

        public Brush ModalBodyBrush { get => ModalBodyBrushVariable.Value; set => ModalBodyBrushVariable.Value = value; }
        private ThemeVariable<Brush> ModalBodyBrushVariable { get; }

        public Brush ModalToolbarBrush { get => ModalToolbarBrushVariable.Value; set => ModalToolbarBrushVariable.Value = value; }
        private ThemeVariable<Brush> ModalToolbarBrushVariable { get; }

        public Brush ModalBorderBrush { get => ModalBorderBrushVariable.Value; set => ModalBorderBrushVariable.Value = value; }
        private ThemeVariable<Brush> ModalBorderBrushVariable { get; }

        // ----------
        // Menu icons
        // ----------

        public ImageSource MenuJamIconSource { get => MenuJamIconSourceVariable.Value; set => MenuJamIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuJamIconSourceVariable { get; }

        public ImageSource MenuRankingIconSource { get => MenuRankingIconSourceVariable.Value; set => MenuRankingIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuRankingIconSourceVariable { get; }

        public ImageSource MenuAwardsIconSource { get => MenuAwardsIconSourceVariable.Value; set => MenuAwardsIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuAwardsIconSourceVariable { get; }

        public ImageSource MenuExportIconSource { get => MenuExportIconSourceVariable.Value; set => MenuExportIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuExportIconSourceVariable { get; }

        public ImageSource MenuPreviewIconSource { get => MenuPreviewIconSourceVariable.Value; set => MenuPreviewIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuPreviewIconSourceVariable { get; }

        public ImageSource MenuSummaryIconSource { get => MenuSummaryIconSourceVariable.Value; set => MenuSummaryIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuSummaryIconSourceVariable { get; }

        public ImageSource MenuFilesIconSource { get => MenuFilesIconSourceVariable.Value; set => MenuFilesIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuFilesIconSourceVariable { get; }

        public ImageSource MenuJampackIconSource { get => MenuJampackIconSourceVariable.Value; set => MenuJampackIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuJampackIconSourceVariable { get; }

        public ImageSource MenuSaveIconSource { get => MenuSaveIconSourceVariable.Value; set => MenuSaveIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuSaveIconSourceVariable { get; }

        public ImageSource MenuSaveModifiedIconSource { get => MenuSaveModifiedIconSourceVariable.Value; set => MenuSaveModifiedIconSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> MenuSaveModifiedIconSourceVariable { get; }

        // -----------
        // Small icons
        // -----------

        public ImageSource IconBinSource { get => IconBinSourceVariable.Value; set => IconBinSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> IconBinSourceVariable { get; }

        public ImageSource IconCenterSource { get => IconCenterSourceVariable.Value; set => IconCenterSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> IconCenterSourceVariable { get; }

        public ImageSource IconCheckSource { get => IconCheckSourceVariable.Value; set => IconCheckSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> IconCheckSourceVariable { get; }

        public ImageSource IconCrossSource { get => IconCrossSourceVariable.Value; set => IconCrossSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> IconCrossSourceVariable { get; }

        public ImageSource IconNilSource { get => IconNilSourceVariable.Value; set => IconNilSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> IconNilSourceVariable { get; }

        public ImageSource IconPreviewSource { get => IconPreviewSourceVariable.Value; set => IconPreviewSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> IconPreviewSourceVariable { get; }

        public ImageSource IconSaveSource { get => IconSaveSourceVariable.Value; set => IconSaveSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> IconSaveSourceVariable { get; }

        // --------------------
        // Miscellaneous images
        // --------------------

        public ImageSource EntryPlaceholderSource { get => EntryPlaceholderSourceVariable.Value; set => EntryPlaceholderSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> EntryPlaceholderSourceVariable { get; }

        public ImageSource StarEmptySource { get => StarEmptySourceVariable.Value; set => StarEmptySourceVariable.Value = value; }
        private ThemeVariable<ImageSource> StarEmptySourceVariable { get; }

        public ImageSource StarFullSource { get => StarFullSourceVariable.Value; set => StarFullSourceVariable.Value = value; }
        private ThemeVariable<ImageSource> StarFullSourceVariable { get; }
    }
}
