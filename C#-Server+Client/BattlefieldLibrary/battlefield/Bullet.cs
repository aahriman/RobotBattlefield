using BaseLibrary.utils.euclidianSpaceStruct;
using BattlefieldLibrary.battlefield.robot;

namespace BattlefieldLibrary.battlefield {
	internal struct Bullet {
	    public readonly int TO_LAP;
	    public readonly int FROM_LAP;
	    public readonly double TO_X;
	    public readonly double TO_Y;
	    public readonly double FROM_X;
	    public readonly double FROM_Y;
	    public readonly Tank TANK;


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
