
package BaseLibrary.equipment;

import java.util.Arrays;

import BaseLibrary.communication.protocol.AProtocol;

public class Gun implements IClassEquipment {

	private static final String NAME = "GUN";

	static {
		AProtocol.registerForDeserialize(NAME, Gun.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	public final int ID;

	public final int COST;

	/**
	 * How many barrel this gun has. Every barrel can be use again after reload time
	 * (<code>Battlefield.RELOAD_TIME</code>)
	 * 
	 * @see BattlefieldLibrary.Battlefield.RELOAD_TIME
	 */
	public final int BARREL_NUMBER;

	/**
	 * How far can gun shoot.
	 */
	public final int MAX_RANGE;

	/**
	 * How fast bullet fly.
	 */
	public final double SHOT_SPEED;

	/**
	 * Effect of bullets. Their damaging and range of effect.
	 */
	public final Zone[] ZONES;

	public Gun(int ID, int COST, int BARREL_NUMBER, int MAX_RANGE, double SHOT_SPEED, Zone[] ZONES) {
		if (ZONES == null)
			throw new IllegalArgumentException("zones cannot be null.");
		this.ID = ID;
		this.COST = COST;
		this.BARREL_NUMBER = BARREL_NUMBER;
		this.MAX_RANGE = MAX_RANGE;
		this.SHOT_SPEED = SHOT_SPEED;
		this.ZONES = new Zone[ZONES.length];
		for (int i = 0; i < ZONES.length; i++) {
			this.ZONES[i] = ZONES[i];
		}
	}

	protected boolean equals(Gun other) {
		if (BARREL_NUMBER != other.BARREL_NUMBER)
			return false;
		if (COST != other.COST)
			return false;
		if (ID != other.ID)
			return false;
		if (MAX_RANGE != other.MAX_RANGE)
			return false;
		if (Double.doubleToLongBits(SHOT_SPEED) != Double.doubleToLongBits(other.SHOT_SPEED))
			return false;
		if (!Arrays.equals(ZONES, other.ZONES))
			return false;
		return true;
	}

	/**
	 * How many barrel this gun has. Every barrel can be use again after reload time
	 * (<code>Battlefield.RELOAD_TIME</code>)
	 * 
	 * @see BattlefieldLibrary.Battlefield.RELOAD_TIME
	 */
	public int getBarrelNumber() {
		return BARREL_NUMBER;
	}

	/**
	 * How far can gun shoot.
	 */
	public int getMaxRange() {
		return MAX_RANGE;
	}

	/**
	 * How fast bullet fly.
	 */
	public double getShotSpeed() {
		return SHOT_SPEED;
	}

	/**
	 * Effect of bullets. Their damaging and range of effect.
	 */
	public Zone[] getZones() {
		return ZONES;
	}

	@Override
	public int getID() {
		return ID;
	}

	@Override
	public int getCost() {
		return COST;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + BARREL_NUMBER;
		result = prime * result + COST;
		result = prime * result + ID;
		result = prime * result + MAX_RANGE;
		long temp;
		temp = Double.doubleToLongBits(SHOT_SPEED);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		result = prime * result + Arrays.hashCode(ZONES);
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
		Gun other = (Gun) obj;
		return equals(other);
	}

	@Override
	public String toString() {
		return "Gun [ID=" + ID + ", COST=" + COST + ", BARREL_NUMBER=" + BARREL_NUMBER + ", MAX_RANGE=" + MAX_RANGE
				+ ", SHOT_SPEED=" + SHOT_SPEED + ", ZONES=" + Arrays.toString(ZONES) + "]";
	}
}
