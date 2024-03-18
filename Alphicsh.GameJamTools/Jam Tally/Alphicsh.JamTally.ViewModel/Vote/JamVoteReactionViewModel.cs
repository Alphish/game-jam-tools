using System.Windows.Media;
using Alphicsh.JamTally.Model.Vote;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Theming;

namespace Alphicsh.JamTally.ViewModel.Vote
{
    public class JamVoteReactionViewModel : WrapperViewModel<JamVoteReaction>
    {
        public JamVoteReactionViewModel(JamVoteReaction model, bool isCounted) : base(model)
        {
            IsCounted = isCounted;
            Foreground = IsCounted ? ThemeManager.Current.HighlightText : ThemeManager.Current.DisabledText;
        }

        public int Value => Model.Value;
        public string Type => Model.Type.Name;
        public string User => Model.User;

        public bool IsCounted { get; }
        public Brush Foreground { get; }
        public string Description => $"+{Value} from {User} ({Type})";
    }
}
