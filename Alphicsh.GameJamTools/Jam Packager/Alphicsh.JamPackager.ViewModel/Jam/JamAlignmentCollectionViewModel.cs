using System.Collections.Generic;
using System.Windows.Input;
using Alphicsh.JamPackager.Model.Jam;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamPackager.ViewModel.Jam
{
    public class JamAlignmentCollectionViewModel : CollectionViewModel<JamAlignmentEditable, JamAlignmentEditableViewModel>
    {
        public static CollectionViewModelStub<JamAlignmentEditable, JamAlignmentEditableViewModel> CollectionStub
            => CollectionViewModelStub.Create((JamAlignmentEditable model) => new JamAlignmentEditableViewModel(model));

        public JamAlignmentCollectionViewModel(IList<JamAlignmentEditable> modelEntries)
            : base(modelEntries, CollectionStub, isImmutable: false)
        {
            AddAlignmentCommand = SimpleCommand.From(AddAlignment);
            RemoveAlignmentCommand = SimpleCommand.WithParameter<JamAlignmentEditableViewModel>(RemoveAlignment);
        }

        public ICommand AddAlignmentCommand { get; }
        private void AddAlignment()
        {
            var alignmentModel = new JamAlignmentEditable
            {
                Id = "example",
                Title = "Example",
                ShortTitle = "Example",
                KeywordsString = "Example",
                Color = "#ffffff"
            };
            var alignmentVm = new JamAlignmentEditableViewModel(alignmentModel);
            Add(alignmentVm);
            CompleteChanges();
        }

        public ICommand RemoveAlignmentCommand { get; }
        private void RemoveAlignment(JamAlignmentEditableViewModel alignmentVm)
        {
            Remove(alignmentVm);
            CompleteChanges();
        }
    }
}
