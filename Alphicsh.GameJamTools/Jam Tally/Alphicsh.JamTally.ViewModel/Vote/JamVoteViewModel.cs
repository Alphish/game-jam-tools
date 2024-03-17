using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;
using Alphicsh.JamTools.Common.Mvvm.NotifiableProperties;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteViewModel : WrapperViewModel<JamVote>
    {
        public static CollectionViewModelStub<JamVote, JamVoteViewModel> CollectionStub { get; }
            = new CollectionViewModelStub<JamVote, JamVoteViewModel>(jamVote => new JamVoteViewModel(jamVote));

        public JamVoteViewModel(JamVote model) : base(model)
        {
            ContentProperty = WrapperProperty.ForMember(this, vm => vm.Model.Content);
            ProcessContentCommand = SimpleCommand.From(ProcessContent);

            HasError = !string.IsNullOrEmpty(Model.Error);
            Message = Model.Error ?? "Processing successful!";

            AwardLines = Model.Awards.Select(award => $"{award.Criterion.Name}: {award.Entry.Line}").ToList();
            EntryLines = CalculateEntryLines();
            ReactionLines = Model.Reactions.Select(reaction => $"+{reaction.Value} {reaction.Name}").ToList();
            ReactionScore = "Reaction score: " + Model.GetReactionScore();
        }

        // ----------------
        // Content handling
        // ----------------

        public WrapperProperty<JamVoteViewModel, string> ContentProperty { get; }
        public string Content { get => ContentProperty.Value; set => ContentProperty.Value = value; }

        public ICommand ProcessContentCommand { get; }
        private void ProcessContent()
        {
            Model.ProcessContent();
            ReloadVote();
            RaisePropertyChanged(nameof(Content));
        }

        // ---------
        // Vote data
        // ---------

        public bool HasError { get; set; }
        public string Message { get; set; }
        public string Voter => Model.Voter ?? "<unknown voter>";

        public IReadOnlyCollection<string> AwardLines { get; set; }
        public IReadOnlyCollection<string> EntryLines { get; set; }
        public IReadOnlyCollection<string> ReactionLines { get; set; }
        public string ReactionScore { get; set; }

        private void ReloadVote()
        {
            HasError = !string.IsNullOrEmpty(Model.Error);
            Message = Model.Error ?? "Processing successful!";
            RaisePropertyChanged(nameof(HasError), nameof(Message), nameof(Voter));

            AwardLines = Model.Awards.Select(award => $"{award.Criterion.Name}: {award.Entry.Line}").ToList();
            RaisePropertyChanged(nameof(AwardLines));

            EntryLines = CalculateEntryLines();
            RaisePropertyChanged(nameof(EntryLines));

            ReactionLines = Model.Reactions.Select(reaction => $"+{reaction.Value} {reaction.Name}").ToList();
            ReactionScore = "Reaction score: " + Model.GetReactionScore();
            RaisePropertyChanged(nameof(ReactionLines), nameof(ReactionScore));
        }

        private IReadOnlyCollection<string> CalculateEntryLines()
        {
            var lines = new List<string>();
            foreach (var entry in Model.Ranking)
            {
                lines.Add(entry.Line);
            }

            if (Model.Unjudged.Count > 0)
            {
                lines.Add("");
                lines.Add("Unjudged entries:");
                foreach (var entry in Model.Unjudged)
                {
                    lines.Add(entry.Line);
                }
            }

            if (Model.Missing.Count > 0)
            {
                lines.Add("");
                lines.Add("Missing entries:");
                foreach (var entry in Model.Missing)
                {
                    lines.Add(entry.Line);
                }
            }
            return lines;
        }
    }
}
