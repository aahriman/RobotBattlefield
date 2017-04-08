using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCapcureBattlefield.battlefield {
    internal class Base {
        public readonly double X;
        public readonly double Y;

        public int Progress { get; set; }
        public int TeamId { get; set; }

        public readonly string TYPE_NAME;

        public Base(double x, double y) {
            TYPE_NAME = GetType().ToString();
            X = x;
            Y = y;
        }
    }
}
