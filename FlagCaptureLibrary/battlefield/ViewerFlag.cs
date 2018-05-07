using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagCaptureLibrary.battlefield {
    public class ViewerFlag {
        /// <summary>
        /// X coordinate
        /// </summary>
        public double X;

        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y;
        /// <summary>
        /// Team id which this flag belongs to
        /// </summary>
        public int TEAM_ID;

        /// <summary>
        /// <code>TYPE_NAME = GetType().ToString()</code> needed for de-serialization.
        /// </summary>
        public readonly string TYPE_NAME;

        public ViewerFlag(double x, double y, int teamId) {
            X = x;
            Y = y;
            TEAM_ID = teamId;

            TYPE_NAME = this.GetType().ToString();
        }
    }
}
