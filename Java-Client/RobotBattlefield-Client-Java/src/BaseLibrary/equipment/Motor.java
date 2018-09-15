package BaseLibrary.equipment;

import BaseLibrary.communication.protocol.AProtocol;

public class Motor implements IEquipment {

	private static final String NAME = "MOTOR";

	static {
		AProtocol.registerForDeserialize(NAME, Motor.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	/**
	 * Max robot's speed in units. Arena is 1000 x 1000 units large.
	 */
	public final double MAX_SPEED;

	/**
	 * In what power can robot make rotation.
	 */
	public final double ROTATE_IN;

	/**
	 * Acceleration of motor in percent of power.
	 */
	public final double ACCELERATION;

	/**
	 * Deceleration of motor in percent of power.
	 */
	public final double DECELERATION;

	/**
	 * Immediately performance from 0. What power motor immediately can get. Power
	 * is always in percentage. And robot go <code>robot.POWER*MAX_SPEED</code>
	 * units per turn.
	 */
	public final double MAX_INITIAL_POWER;

	public final int COST;

	public final int ID;

	public Motor(int ID, int COST, double MAX_SPEED, double ROTATE_IN, double ACCELERATION, double DECELERATION,
			double MAX_INITIAL_POWER) {
		this.MAX_SPEED = MAX_SPEED;
		this.ROTATE_IN = ROTATE_IN;
		this.ACCELERATION = ACCELERATION;
		this.DECELERATION = DECELERATION;
		this.MAX_INITIAL_POWER = MAX_INITIAL_POWER;
		this.COST = COST;
		this.ID = ID;
	}
	
	/**
	 * Max robot's speed in units. Arena is 1000 x 1000 units large.
	 */
	public final double getMaxSpeed() {
		return MAX_SPEED;
	}

	/**
	 * In what power can robot make rotation.
	 */
	public final double getRotateIn() { 
		return ROTATE_IN;
	}

	/**
	 * Acceleration of motor in percent of power.
	 */
	public final double getAcceleration() {
		return ACCELERATION;
	}

	/**
	 * Deceleration of motor in percent of power.
	 */
	public final double getDeceleration() {
		return DECELERATION;
	}

	/**
	 * Immediately performance from 0. What power motor immediately can get. Power
	 * is always in percentage. And robot go <code>robot.POWER*MAX_SPEED</code>
	 * units per turn.
	 */
	public final double getMaxInitialPower() {
		return MAX_INITIAL_POWER;
	}
	
	@Override
	public final int getCost() {
		return COST;
	}
	@Override
	public final int getID() {
		return ID;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		long temp;
		temp = Double.doubleToLongBits(ACCELERATION);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		result = prime * result + COST;
		temp = Double.doubleToLongBits(DECELERATION);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		result = prime * result + ID;
		temp = Double.doubleToLongBits(MAX_INITIAL_POWER);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(MAX_SPEED);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(ROTATE_IN);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		return result;
	}

	public boolean equals(Motor other) {
		if (Double.doubleToLongBits(ACCELERATION) != Double.doubleToLongBits(other.ACCELERATION))
			return false;
		if (COST != other.COST)
			return false;
		if (Double.doubleToLongBits(DECELERATION) != Double.doubleToLongBits(other.DECELERATION))
			return false;
		if (ID != other.ID)
			return false;
		if (Double.doubleToLongBits(MAX_INITIAL_POWER) != Double.doubleToLongBits(other.MAX_INITIAL_POWER))
			return false;
		if (Double.doubleToLongBits(MAX_SPEED) != Double.doubleToLongBits(other.MAX_SPEED))
			return false;
		if (Double.doubleToLongBits(ROTATE_IN) != Double.doubleToLongBits(other.ROTATE_IN))
			return false;
		return true;
	}
	
	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Motor other = (Motor) obj;
		return equals(other);
	}

	@Override
	public String toString() {
		return "Motor [MAX_SPEED=" + MAX_SPEED + ", ROTATE_IN=" + ROTATE_IN + ", ACCELERATION=" + ACCELERATION
				+ ", DECELERATION=" + DECELERATION + ", MAX_INITIAL_POWER=" + MAX_INITIAL_POWER + ", COST=" + COST
				+ ", ID=" + ID + "]";
	}

}
