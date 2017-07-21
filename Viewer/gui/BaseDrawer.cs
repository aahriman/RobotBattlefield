using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BaseCapcureBattlefieldLibrary.battlefield;
using Viewer.utils;
using ViewerLibrary;

namespace Viewer.gui {
    public class BaseDrawer : IDrawerMore {


        const float BASE_SIZE = 20;

        private readonly Pen innerBasePen = new Pen(Color.LightGray);

        public void DrawMore(object[] more, Graphics g) {
            Base[] bases = more as Base[];
            if (bases == null) {
                return;
            }
            foreach (var @base in bases) {
                drawBase(@base, g);
            }
        }

        private void drawBase(Base @base, Graphics g) {
            float x = (float) (@base.X - BASE_SIZE / 2.0);
            float y = (float) (@base.Y - BASE_SIZE / 2.0);
            g.FillEllipse(innerBasePen.Brush, x, y, BASE_SIZE, BASE_SIZE);
            //g.DrawEllipse(getTeamPen(@base.TeamId), x, y, BASE_SIZE, BASE_SIZE);
            

            Pen teamPen = DefaultDrawer.GetTeamPen(@base.ProgressTeamId);

            
            float progressSize = BASE_SIZE * @base.Progress / @base.MAX_PROGRESS;
            if (progressSize > 0) {
                x = (float) (@base.X - progressSize / 2.0);
                y = (float) (@base.Y - progressSize / 2.0);
                g.FillEllipse(teamPen.Brush, x, y, progressSize, progressSize);
            }
        }
    }
}
