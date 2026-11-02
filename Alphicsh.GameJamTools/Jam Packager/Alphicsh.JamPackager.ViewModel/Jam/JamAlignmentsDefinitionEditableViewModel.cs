using System;
using System.Collections.Generic;
using System.Linq;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamAlignmentsDefinitionEditableViewModel : WrapperViewModel<JamAlignmentsDefinitionEditable>
    {
        public JamAlignmentsDefinitionEditableViewModel(JamAlignmentsDefinitionEditable model) : base(model)
        {
            NeitherTitleProperty = WrapperProperty.ForMember(this, vm => vm.Model.NeitherTitle);
            Alignments = new JamAlignmentCollectionViewModel(model.Alignments);

            AvailableAlignmentsProperty = NotifiableProperty.Create(this, nameof(AvailableAlignments));
            AvailableAlignments = new List<JamAlignmentChoiceViewModel>();
            NeitherTitleProperty.PropertyChanged += RegenerateAlignmentOptions;
            Alignments.CollectionChanged += (sender, e) =>
            {
                UnobserveAlignmentItems();
                RegenerateAlignmentOptions(sender, e);
                ObserveAlignmentItems();
            };

            RegenerateAlignmentOptions(null, EventArgs.Empty);
            ObserveAlignmentItems();
        }

        private void RegenerateAlignmentOptions(object? sender, EventArgs e)
        {
            var result = new List<JamAlignmentChoiceViewModel>();
            result.Add(new JamAlignmentChoiceViewModel(option: null, NeitherTitle));
            foreach (var alignment in Alignments)
            {
                result.Add(new JamAlignmentChoiceViewModel(alignment.Model, alignment.Model.Title));
            }
            AvailableAlignments = result;
            AvailableAlignmentsProperty.RaisePropertyChanged();
        }

        private void ObserveAlignmentItems()
        {
            var alignmentsById = Alignments.ToDictionary(alignment => alignment.Id);
            foreach (var choice in AvailableAlignments)
            {
                if (choice.Id == null)
                    continue;

                var alignment = alignmentsById[choice.Id];
                alignment.PropertyChanged += RegenerateAlignmentOptions;
            }
        }

        private void UnobserveAlignmentItems()
        {
            var alignmentsById = Alignments.ToDictionary(alignment => alignment.Id);
            foreach (var choice in AvailableAlignments)
            {
                if (choice.Id == null)
                    return;

                var alignment = alignmentsById[choice.Id];
                alignment.PropertyChanged -= RegenerateAlignmentOptions;
            }
        }

        public WrapperProperty<JamAlignmentsDefinitionEditableViewModel, string> NeitherTitleProperty { get; }
        public string NeitherTitle { get => NeitherTitleProperty.Value; set => NeitherTitleProperty.Value = value; }

        public JamAlignmentCollectionViewModel Alignments { get; }

        public NotifiableProperty AvailableAlignmentsProperty { get; }
        public List<JamAlignmentChoiceViewModel> AvailableAlignments { get; set; }
    }
}
