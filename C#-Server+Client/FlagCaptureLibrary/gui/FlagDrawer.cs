using System.Drawing;
using FlagCaptureLibrary.battlefield;
using ViewerLibrary.gui;

namespace FlagCaptureLibrary.gui {
    /// <summary>
    /// Draw flag and FlagPlace to arena. Use-full for CaptureTheFlagBattlefield.
    /// </summary>
    public class FlagDrawer : IDrawerMore {
        private const int FLAG_SIZE = 10;
        private const int FLAG_PLACE_SIZE = 15;


        public void DrawMore(object[] more, Graphics g) {

            foreach (var o in more) {
                drawFlags(o as ViewerFlag, g);
                drawFlagPlace(o as FlagPlace, g);
            }
        }

        private void drawFlags(ViewerFlag flag, Graphics g) {
            if (flag == null) return;
            Pen teamPen = DefaultDrawer.GetTeamPen(flag.TEAM_ID);
            lock (g) {
                lock (teamPen) {
                    g.FillRectangle(teamPen.Brush, (float) (flag.X - FLAG_SIZE / 2.0), (float) (flag.Y - FLAG_SIZE / 2.0), FLAG_SIZE, FLAG_SIZE);
                }
            }
        }

        private void drawFlagPlace(FlagPlace flagPlace, Graphics g) {
            if (flagPlace == null) return;

            Pen teamPen = DefaultDrawer.GetTeamPen(flagPlace.TEAM_ID);
            lock (g) {
                lock (teamPen) {
                    g.FillEllipse(teamPen.Brush, (float) (flagPlace.X - FLAG_PLACE_SIZE / 2.0), (float) (flagPlace.Y - FLAG_PLACE_SIZE / 2.0), FLAG_PLACE_SIZE, FLAG_PLACE_SIZE);
                }
            }
        }
    }
}
