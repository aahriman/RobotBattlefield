using BaseLibrary.utils.euclidianSpaceStruct;
using BattlefieldLibrary.battlefield.robot;

namespace BattlefieldLibrary.battlefield {
    public class Mine {
        public int ID { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public MineLayer MineLayer { get; private set; }

        public Mine(int id, double x, double y, MineLayer mineLayer) {
            ID = id;
            X = x;
            Y = y;
            MineLayer = mineLayer;
        }

        public Point GetPosition() {
            return new Point(X, Y);
        }
    }
}
