using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphicsh.JamPlayer.Model.Ratings
{
    public class RatingCriteriaOverview
    {
        public IReadOnlyCollection<IRatingSkin> Skins { get; set; } = default!;
        public IReadOnlyCollection<IRatingCriterion> Criteria { get; set; } = default!;
    }
}
