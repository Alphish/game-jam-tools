using System.Collections.Generic;
using System.Drawing;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class TrophiesComposite
    {
        public TrophiesImage Image { get; init; } = default!;
        public string Id { get; init; } = default!;
        public int X { get; init; }
        public int Y { get; init; }

        public IReadOnlyCollection<TrophiesGuide> Guides => GuidesList;
        private List<TrophiesGuide> GuidesList { get; } = new List<TrophiesGuide>();
        private Dictionary<string, TrophiesGuide> GuidesByRole { get; } = new Dictionary<string, TrophiesGuide>();

        public static TrophiesComposite Create(TrophiesImage image, string id, GameTrophyLayout layout)
        {
            return new TrophiesComposite
            {
                Image = image,
                Id = id,
                X = layout.X,
                Y = layout.Y,
            };
        }

        public TrophiesComposite WithGuide(string layer, string role, Rectangle area, string fill, decimal opacity = 0.1m)
            => WithGuide(Image.FindLayer(layer), role, area, fill, opacity);

        public TrophiesComposite WithGuide(TrophiesLayer layer, string role, Rectangle area, string fill, decimal opacity = 0.1m)
        {
            var guide = TrophiesGuide.FindOrCreate(layer, this, role, area, fill, opacity);
            GuidesList.Add(guide);
            GuidesByRole.Add(guide.Role, guide);
            return this;
        }

        public TrophiesComposite CloneTo(string role, int x, int y)
        {
            GuidesByRole[role].CloneTo(x, y);
            return this;
        }

        public TrophiesComposite CloneWithText(string role, int x, int y, string text)
        {
            GuidesByRole[role].CloneWithText(x, y, text);
            return this;
        }
    }
}
