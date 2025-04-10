using System.Drawing;

namespace Alphicsh.JamTally.Trophies.Image
{
    public class GameTrophyLayout
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle JamLabelArea { get; }
        public Rectangle MedalArea { get; }

        public Rectangle AuthorsArea { get; }
        public Rectangle TitleArea { get; }
        public Rectangle DescriptionArea { get; }

        public GameTrophyLayout(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            JamLabelArea = new Rectangle(X, Y, 20, Height);
            MedalArea = new Rectangle(JamLabelArea.Right, Y, 120, 120);

            var preTextWidth = JamLabelArea.Width + MedalArea.Width;
            AuthorsArea = new Rectangle(MedalArea.Right, Y, Width - preTextWidth, 30);
            TitleArea = new Rectangle(MedalArea.Right, AuthorsArea.Bottom, Width - preTextWidth, 60);
            DescriptionArea = new Rectangle(MedalArea.Right, TitleArea.Bottom, Width - preTextWidth, 30);
        }
    }
}
