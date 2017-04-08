namespace CommunicationLibrary.equip {
	public class Motor {
		public double MAX_SPEED { get; private set; }
		public double ROTATE_IN { get; private set; }
		public double SPEED_UP { get; private set; }
		public double SPEED_DOWN { get; private set; }
		public double SPEED_UP_TO { get; private set; }
		public int COST { get; private set; }
		public int ID { get; private set; }

		public Motor(double maxSpeed, double rotateIn, double speedUp, double speedDown, double speedUpTo, int cost, int id) {
			MAX_SPEED = maxSpeed;
			ROTATE_IN = rotateIn;
			SPEED_UP = speedUp;
			SPEED_DOWN = speedDown;
			SPEED_UP_TO = speedUpTo;
			COST = cost;
			ID = id;
		}

		public override bool Equals(object obj) {
			Motor m = obj as Motor;
			return m != null &&
				MAX_SPEED == m.MAX_SPEED &&
				ROTATE_IN == m.ROTATE_IN &&
				SPEED_UP == m.SPEED_UP &&
				SPEED_DOWN == m.SPEED_DOWN &&
				SPEED_UP_TO == m.SPEED_UP_TO &&
				COST == m.COST &&
				ID == m.ID;
		}
	}
}
