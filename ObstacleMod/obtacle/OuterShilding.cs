using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace ObstacleMod.obtacle {
    public class OuterShilding : IScanInfluence {
        public string TypeName => this.GetType().ToString();

        public int X { get; }
        public int Y { get; }
        public bool Used { get; set; }

        public OuterShilding(int x, int y) {
            X = x;
            Y = y;
        }

        public bool CanScan(int turn, double fromX, double fromY, double toX, double toY) {
            return X < fromX && fromX < X + 1 && Y < fromY && fromY < Y + 1;
        }

        public void Draw(Graphics graphics, float xScale, float yScale) {
            // TODO
        }
    }
}
