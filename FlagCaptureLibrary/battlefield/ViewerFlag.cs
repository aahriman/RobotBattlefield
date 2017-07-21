using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagCaptureLibrary.battlefield {
    public class ViewerFlag {
        public double X;
        public double Y;
        /// <summary>
        /// Team id whitch this flag belongs to
        /// </summary>
        public int TEAM_ID;

        public readonly string TYPE_NAME;

        public ViewerFlag(double x, double y, int teamId) {
            X = x;
            Y = y;
            TEAM_ID = teamId;

            TYPE_NAME = this.GetType().ToString();
        }
    }
}
