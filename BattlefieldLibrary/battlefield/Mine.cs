using BaseLibrary.utils.euclidianSpaceStruct;
using BattlefieldLibrary.battlefield.robot;

namespace BattlefieldLibrary.battlefield {
    public class Mine {
        public int ID { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public Miner MINER { get; private set; }

        public Mine(int id, double x, double y, Miner miner) {
            ID = id;
            X = x;
            Y = y;
            MINER = miner;
        }

        public Point GetPosition() {
            return new Point(X, Y);
        }
    }
}
