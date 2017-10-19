using System;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace FlagCaptureLibrary.battlefield {
    public class FlagPlace {
        private static int id = 1;
        public readonly int TEAM_ID;

        public readonly int ID;


        public double X { get; internal set; }
        public double Y { get; internal set; }

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
