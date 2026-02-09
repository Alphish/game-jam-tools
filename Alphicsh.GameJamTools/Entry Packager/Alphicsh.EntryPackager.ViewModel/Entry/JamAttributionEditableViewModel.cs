using System.Collections.Generic;
using System.Windows.Input;
using Alphicsh.EntryPackager.Model.Entry;
using Alphicsh.JamTools.Common.IO.Jam.Entries;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.EntryPackager.ViewModel.Entry
{
    public class JamAttributionEditableViewModel : WrapperViewModel<JamAttributionEditable>
    {
        public JamAttributionEditableViewModel(JamAttributionEditable model) : base(model)
        {
            CodeProperty = WrapperProperty.ForMember(this, vm => vm.Model.Code);
            VisualsProperty = WrapperProperty.ForMember(this, vm => vm.Model.Visuals);
            MusicProperty = WrapperProperty.ForMember(this, vm => vm.Model.Music);
            SoundsProperty = WrapperProperty.ForMember(this, vm => vm.Model.Sounds);

            AttributionStringProperty = NotifiableProperty.Create(this, nameof(AttributionString))
                .DependingOn(CodeProperty, VisualsProperty, MusicProperty, SoundsProperty);

            OpenGuideCommand = SimpleCommand.From(OpenGuide);
        }

        public WrapperProperty<JamAttributionEditableViewModel, ComponentAttribution> CodeProperty { get; }
        public ComponentAttribution Code { get => CodeProperty.Value; set => CodeProperty.Value = value; }

        public WrapperProperty<JamAttributionEditableViewModel, ComponentAttribution> VisualsProperty { get; }
        public ComponentAttribution Visuals { get => VisualsProperty.Value; set => VisualsProperty.Value = value; }

        public WrapperProperty<JamAttributionEditableViewModel, ComponentAttribution> MusicProperty { get; }
        public ComponentAttribution Music { get => MusicProperty.Value; set => MusicProperty.Value = value; }

        public WrapperProperty<JamAttributionEditableViewModel, ComponentAttribution> SoundsProperty { get; }
        public ComponentAttribution Sounds { get => SoundsProperty.Value; set => SoundsProperty.Value = value; }

        public static IReadOnlyCollection<ComponentAttributionEntry> AvailableAttributionTypes { get; } = new List<ComponentAttributionEntry>()
        {
            new ComponentAttributionEntry("Unspecified", ComponentAttribution.Unspecified),
            new ComponentAttributionEntry("None", ComponentAttribution.None),
            new ComponentAttributionEntry("Premade", ComponentAttribution.Premade),
            new ComponentAttributionEntry("Mostly Premade", ComponentAttribution.MostlyPremade),
            new ComponentAttributionEntry("Partly Premade", ComponentAttribution.PartPremade),
            new ComponentAttributionEntry("Mostly Custom", ComponentAttribution.MostlyCustom),
            new ComponentAttributionEntry("Custom", ComponentAttribution.Custom),
        };

        public NotifiableProperty AttributionStringProperty { get; }
        public string AttributionString
            => $"{AttributionModifiers[Code]} code | {AttributionModifiers[Visuals]} visuals | {AttributionModifiers[Music]} music | {AttributionModifiers[Sounds]} sounds";

        private static IReadOnlyDictionary<ComponentAttribution, string> AttributionModifiers { get; } = new Dictionary<ComponentAttribution, string>
        {
            [ComponentAttribution.Unspecified] = "Unspecified",
            [ComponentAttribution.None] = "No",
            [ComponentAttribution.Premade] = "Premade",
            [ComponentAttribution.MostlyPremade] = "Mostly premade",
            [ComponentAttribution.PartPremade] = "Partly premade",
            [ComponentAttribution.MostlyCustom] = "Mostly custom",
            [ComponentAttribution.Custom] = "Custom",
        };

        public ICommand OpenGuideCommand { get; }
        private void OpenGuide()
            => AttributionGuideViewModel.ShowModal();
    }
}
