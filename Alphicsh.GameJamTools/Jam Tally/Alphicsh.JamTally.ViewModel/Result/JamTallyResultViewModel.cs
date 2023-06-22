using System.Windows.Input;
using Alphicsh.JamTally.Model.Result;
using Alphicsh.JamTools.Common.Mvvm;
using Alphicsh.JamTools.Common.Mvvm.Commands;

namespace Alphicsh.JamTally.ViewModel.Result
{
    public class JamTallyResultViewModel : WrapperViewModel<JamTallyResult>
    {
        public JamTallyResultViewModel(JamTallyResult model) : base(model)
        {
            FinalRankingText = model.GetFinalRankingText();
            AwardRankingsText = model.GetAwardRankingsText();

            GenerateResultsPostCommand = SimpleCommand.From(GenerateResultsPost);
        }

        public string FinalRankingText { get; }
        public string AwardRankingsText { get; }

        // ----------
        // Generators
        // ----------

        public string ResultsPostText { get; private set; }
        public ICommand GenerateResultsPostCommand { get; }
        private void GenerateResultsPost()
        {
            ResultsPostText = Model.GenerateResultsPost();
            RaisePropertyChanged(nameof(ResultsPostText));
        }
    }
}
