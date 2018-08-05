using System;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace FlagCaptureLibrary.battlefield {
    public class FlagPlace {
        private static int id = 1;

        /// <summary>
        /// FlagPlace belong to team witch id is specified here.
        /// </summary>
        public readonly int TEAM_ID;

        /// <summary>
        /// Flag place id.
        /// </summary>
        public readonly int ID;


        /// <summary>
        /// X coordinate
        /// </summary>
        public double X { get; internal set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get; internal set; }

        /// <summary>
        /// Type name for de-serialization used.
        /// </summary>
        public readonly string TYPE_NAME;

        public FlagPlace(double X, double Y, int TEAM_ID) {
            ID = FlagPlace.id++;
            this.X = X;
            this.Y = Y;
            this.TEAM_ID = TEAM_ID;

            TYPE_NAME = GetType().ToString();
        }

        public Point GetPosition() {
            return new Point(X, Y);
        }
    }
}
