using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlagCaptureLibrary.battlefield;
using Viewer.utils;
using ViewerLibrary.gui;

namespace Viewer.gui {
    public class FlagDrawer : IDrawerMore {
        private const int FLAG_SIZE = 10;
        private const int FLAG_PLACE_SIZE = 15;


        public void DrawMore(object[] more, Graphics g) {
            ViewerFlag[] flags = more as ViewerFlag[];
            if (flags != null) {
                drawFlags(flags, g);
            }

            FlagPlace[] flagPlaces= more as FlagPlace[];
            if (flagPlaces != null) {
                drawFlagPlaces(flagPlaces, g);
            }
        }

        public void drawFlags(ViewerFlag[] flags, Graphics g) {
            foreach (var flag in flags) {
                Pen teamPen = DefaultDrawer.GetTeamPen(flag.TEAM_ID);
                g.FillRectangle(teamPen.Brush, (float)(flag.X - FLAG_SIZE / 2.0), (float)(flag.Y - FLAG_SIZE / 2.0), FLAG_SIZE, FLAG_SIZE);
            }
        }
        
        public void drawFlagPlaces(FlagPlace[] flagPlaces, Graphics g) {
            foreach (var flagPlace in flagPlaces) {
                Pen teamPen = DefaultDrawer.GetTeamPen(flagPlace.TEAM_ID);
                g.FillEllipse(teamPen.Brush, (float)(flagPlace.X - FLAG_PLACE_SIZE/2.0), (float)(flagPlace.Y - FLAG_PLACE_SIZE/2.0), FLAG_PLACE_SIZE, FLAG_PLACE_SIZE);
            }
        }

    }
}
