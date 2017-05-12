using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace FlagCapcureBattlefield.battlefield {
    public class FlagPlace {
        private static int id = 1;
        public readonly string TYPE_NAME;
        public readonly int TEAM_ID;

        public readonly int ID;


        public double X { get; internal set; }
        public double Y { get; internal set; }

         

        public FlagPlace(double X, double Y, int TEAM_ID) {
            ID = FlagPlace.id++;
            TYPE_NAME = GetType().ToString();
            this.X = X;
            this.Y = Y;
            this.TEAM_ID = TEAM_ID;
        }

        public Point GetPosition() {
            return new Point(X, Y);
        }
    }
}
