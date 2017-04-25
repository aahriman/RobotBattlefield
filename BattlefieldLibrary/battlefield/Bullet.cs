using BaseLibrary.utils.euclidianSpaceStruct;
using BattlefieldLibrary.battlefield.robot;

namespace BattlefieldLibrary.battlefield {
	internal class Bullet {
		public int TO_LAP { get; private set; }
		public int FROM_LAP { get; private set; }
		public double TO_X { get; private set; }
		public double TO_Y { get; private set; }
		public double FROM_X { get; private set; }
		public double FROM_Y { get; private set; }
		public Tank TANK { get; private set; }


		public Bullet(int fromLap, int toLap, double toX, double toY, Tank tank) {
			FROM_LAP = fromLap;
			TO_LAP = toLap;
			TO_X = toX;
			TO_Y = toY;
			FROM_X = tank.X;
			FROM_Y = tank.Y;
			TANK = tank;
		}

	    public Point GetToPosition() {
	        return new Point(TO_X, TO_Y);
	    }

        public Point GetFromPosition() {
            return new Point(FROM_X, FROM_Y);
        }
    }
}
