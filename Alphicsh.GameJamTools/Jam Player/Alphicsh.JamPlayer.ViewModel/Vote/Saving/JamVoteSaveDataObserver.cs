using Alphicsh.JamPlayer.ViewModel.Awards;
using Alphicsh.JamPlayer.ViewModel.Ranking;
using Alphicsh.JamPlayer.ViewModel.Ratings;
using Alphicsh.JamTools.Common.Mvvm.Observation;
using Alphicsh.JamTools.Common.Mvvm.Saving;

namespace Alphicsh.JamPlayer.ViewModel.Vote.Saving
{
    public class JamVoteSaveDataObserver : SaveDataObserver<JamVoteViewModel>
    {
        protected override IObserverNode CreateInnerObserver(JamVoteViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingViewModel(ObserveRanking(viewModel.Ranking))
                .ObservingViewModel(ObserveAwards(viewModel.Awards));
        }

        // -------
        // Ranking
        // -------

        private IObserverNode ObserveRanking(RankingOverviewViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingProperty(viewModel.PendingCountProperty)
                .ObservingCollection(viewModel.UnrankedEntries, ObserveEntry)
                .ObservingCollection(viewModel.RankedEntries, ObserveEntry);
        }

        private IObserverNode ObserveEntry(RankingEntryViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingProperty(viewModel.IsUnjudgedProperty)
                .ObservingViewModelsIn(viewModel.Ratings, ObserveRating)
                .ObservingProperty(viewModel.CommentProperty);
        }

        private IObserverNode ObserveRating(RatingViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingProperty(viewModel.GenericValueProperty);
        }

        // ------
        // Awards
        // ------

        private IObserverNode ObserveAwards(AwardsOverviewViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingViewModelsIn(viewModel.Entries, ObserveAwardEntry)
                .ObservingProperty(viewModel.BestReviewerProperty);
        }

        private IObserverNode ObserveAwardEntry(AwardEntryViewModel viewModel)
        {
            return CreateViewModelObserver()
                .ObservingProperty(viewModel.EntryProperty);
        }
    }
}
