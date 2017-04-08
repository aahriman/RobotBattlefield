using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagCapcureBattlefield.battlefield {
    public class Flag {
        public readonly string TYPE_NAME;
        public readonly int TEAM_ID;


        public double X { get; internal set; }
        public double Y { get; internal set; }

         

        public Flag(double X, double Y, int TEAM_ID) {
            TYPE_NAME = GetType().ToString();
            this.X = X;
            this.Y = Y;
            this.TEAM_ID = TEAM_ID;
        }
    }
}
