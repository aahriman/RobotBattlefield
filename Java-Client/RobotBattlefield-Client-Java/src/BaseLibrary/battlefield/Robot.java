package BaseLibrary.battlefield;

import BaseLibrary.equipment.Armor;
import BaseLibrary.equipment.Motor;
import BaseLibrary.utils.euclidianSpaceStruct.Point;

public abstract class Robot {

        /**
        * Robot id.
        */
        public int ID;

        /**
        * Robot team.
        */
        public int TEAM_ID;


        /**
        * HitPoints which robot has.
        */
        public int HitPoints;

        /**
        * Score which robot has.
        */
        public int Score;

        /**
        * Gold which robot has.
        */
        public int Gold;

        /**
        * X-coordinate of robot.
        */
        public double X;

        /**
        * Y-coordinate of robot.
        */
        public double Y;

        /**
        * Power which robot's motor currently use.
        */
        public double Power;

        /**
        * Angle where robot goes.
        */
        public double AngleDrive;

        /**
        * What motor do robot has.
        * @see Motor
        */
        public Motor Motor;

        /**
        * What armor do robot has.
        * @see Armor
        */
        public Armor Armor;


        public Robot() {
        }
        
        /**
         * Robot's position 
         * @return
         */
        public Point getPosition() {
        	return new Point(X, Y);
        }

        @Override
        public String toString() {
            return String.format("Robot: {{ ID: %d, HitPoints: %d, Position: [%d, %d]",ID, HitPoints, X,Y);
        }


		@Override
		public int hashCode() {
			final int prime = 31;
			int result = 1;
			long temp;
			temp = Double.doubleToLongBits(AngleDrive);
			result = prime * result + (int) (temp ^ (temp >>> 32));
			result = prime * result + ((Armor == null) ? 0 : Armor.hashCode());
			result = prime * result + Gold;
			result = prime * result + HitPoints;
			result = prime * result + ID;
			result = prime * result + ((Motor == null) ? 0 : Motor.hashCode());
			temp = Double.doubleToLongBits(Power);
			result = prime * result + (int) (temp ^ (temp >>> 32));
			result = prime * result + Score;
			result = prime * result + TEAM_ID;
			temp = Double.doubleToLongBits(X);
			result = prime * result + (int) (temp ^ (temp >>> 32));
			temp = Double.doubleToLongBits(Y);
			result = prime * result + (int) (temp ^ (temp >>> 32));
			return result;
		}


		@Override
		public boolean equals(Object obj) {
			if (this == obj)
				return true;
			if (obj == null)
				return false;
			if (getClass() != obj.getClass())
				return false;
			Robot other = (Robot) obj;
			if (Double.doubleToLongBits(AngleDrive) != Double.doubleToLongBits(other.AngleDrive))
				return false;
			if (Armor == null) {
				if (other.Armor != null)
					return false;
			} else if (!Armor.equals(other.Armor))
				return false;
			if (Gold != other.Gold)
				return false;
			if (HitPoints != other.HitPoints)
				return false;
			if (ID != other.ID)
				return false;
			if (Motor == null) {
				if (other.Motor != null)
					return false;
			} else if (!Motor.equals(other.Motor))
				return false;
			if (Double.doubleToLongBits(Power) != Double.doubleToLongBits(other.Power))
				return false;
			if (Score != other.Score)
				return false;
			if (TEAM_ID != other.TEAM_ID)
				return false;
			if (Double.doubleToLongBits(X) != Double.doubleToLongBits(other.X))
				return false;
			if (Double.doubleToLongBits(Y) != Double.doubleToLongBits(other.Y))
				return false;
			return true;
		}

		public static enum RobotType {
	        REPAIRMAN, TANK, MINE_LAYER, NONE // none is not defined type
	    }
    }
