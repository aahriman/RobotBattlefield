using System.Drawing;
using BaseCaptureLibrary.battlefield;
using ViewerLibrary.gui;
using ViewerLibrary.utils;

namespace BaseCaptureLibrary.gui {
    public class BaseDrawer : IDrawerMore {
        const float BASE_SIZE = 25;


        private readonly Pen BLACK_PEN = new Pen(Color.Black);
        private readonly Pen INNER_BASE_PEN = new Pen(Color.LightGray);

        public void DrawMore(object[] more, Graphics g) {
            if(more == null) return;
            foreach (var o in more) {
                Base @base = o as Base;
                drawBase(@base, g);
            }
        }

        private void drawBase(Base @base, Graphics g) {
            if (@base == null) return;

            float x = (float) (@base.X - BASE_SIZE / 2.0);
            float y = (float) (@base.Y - BASE_SIZE / 2.0);

            
            float progressSize = (BASE_SIZE - 5) * @base.Progress / @base.MAX_PROGRESS;
            Pen teamPen = DefaultDrawer.GetTeamPen(@base.TeamId);
            Font drawFont = new Font("Arial", 10);
            string progressText = $"{@base.Progress}";
            
            lock (g) {
                lock (INNER_BASE_PEN) {
                    g.FillEllipse(INNER_BASE_PEN.Brush, x, y, BASE_SIZE, BASE_SIZE);
                }
                lock (teamPen) {
                    float oldWidth = teamPen.Width;
                    teamPen.Width = 5;
                    g.DrawEllipse(teamPen, x, y, BASE_SIZE, BASE_SIZE);
                    teamPen.Width = oldWidth;
                }
            }

            if (progressSize > 0) {
                x = (float) (@base.X - progressSize / 2.0);
                y = (float) (@base.Y - progressSize / 2.0);
                Pen progressTeamPen = DefaultDrawer.GetTeamPen(@base.ProgressTeamId);
                lock (g) {
                    lock (progressTeamPen) {
                        Brush b = new SolidBrush(ColorUtils.ColorWithAlpha(progressTeamPen.Color, 100));
                        g.FillEllipse(b, x, y, progressSize, progressSize);
                    }
                    SizeF progressTextSize = g.MeasureString(progressText, drawFont);
                    lock (BLACK_PEN) {
                        g.DrawString(progressText, drawFont, BLACK_PEN.Brush, (float) (@base.X - progressTextSize.Width/2), (float) (@base.Y - progressTextSize.Height / 2));
                    }
                }
                
            }
        }
    }
}
