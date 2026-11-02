using System;
using System.Windows.Media;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamAlignmentEditableViewModel : WrapperViewModel<JamAlignmentEditable>
    {
        public JamAlignmentEditableViewModel(JamAlignmentEditable model) : base(model)
        {
            IdProperty = WrapperProperty.ForMember(this, vm => vm.Model.Id);
            TitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.Title);
            ShortTitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.ShortTitle);
            KeywordsStringProperty = WrapperProperty.ForMember(this, vm => vm.Model.KeywordsString);
            ColorStringProperty = WrapperProperty.Create(this, nameof(ColorString), vm => vm.Model.Color, (vm, value) => vm.Model.Color = value);
            BrushProperty = NotifiableProperty.Create(this, nameof(Brush));

            ColorStringProperty.AddDependingProperty(BrushProperty);
        }

        public WrapperProperty<JamAlignmentEditableViewModel, string> IdProperty { get; }
        public string Id { get => IdProperty.Value; set => IdProperty.Value = value; }

        public WrapperProperty<JamAlignmentEditableViewModel, string> TitleProperty { get; }
        public string Title { get => TitleProperty.Value; set => TitleProperty.Value = value; }

        public WrapperProperty<JamAlignmentEditableViewModel, string> ShortTitleProperty { get; }
        public string ShortTitle { get => ShortTitleProperty.Value; set => ShortTitleProperty.Value = value; }

        public WrapperProperty<JamAlignmentEditableViewModel, string?> KeywordsStringProperty { get; }
        public string? KeywordsString { get => KeywordsStringProperty.Value; set => KeywordsStringProperty.Value = value; }

        public WrapperProperty<JamAlignmentEditableViewModel, string?> ColorStringProperty { get; }
        public string? ColorString { get => ColorStringProperty.Value; set => ColorStringProperty.Value = value; }

        public NotifiableProperty BrushProperty { get; }
        public Brush? Brush
        {
            get
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(ColorString);
                    return new SolidColorBrush(color);
                }
                catch (FormatException)
                {
                    return null;
                }
            }
        }
    }
}
