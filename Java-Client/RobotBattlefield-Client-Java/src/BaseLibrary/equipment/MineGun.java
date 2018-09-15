
package BaseLibrary.equipment;

import java.util.Arrays;

import BaseLibrary.communication.protocol.AProtocol;

public class MineGun implements IClassEquipment {
	private static final String NAME = "MINE_GUN";

	static {
		AProtocol.registerForDeserialize(NAME, MineGun.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	public final int ID;

	public final int COST;

	/**
	 * How many mines can be put on map at the same time.
	 */
	public final int MAX_MINES;

	/**
	 * Effect of mines. Their damaging and range of effect.
	 */
	public final Zone[] ZONES;

	public MineGun(int ID, int COST, int MAX_MINES, Zone[] ZONES) {
		if (ZONES == null)
			throw new IllegalArgumentException("zones cannot be null.");
		this.ID = ID;
		this.COST = COST;
		this.MAX_MINES = MAX_MINES;
		this.ZONES = new Zone[ZONES.length];
		for (int i = 0; i < ZONES.length; i++) {
			this.ZONES[i] = ZONES[i];
		}
	}

	protected boolean equals(MineGun other) {
		if (COST != other.COST)
			return false;
		if (ID != other.ID)
			return false;
		if (MAX_MINES != other.MAX_MINES)
			return false;
		if (!Arrays.equals(ZONES, other.ZONES))
			return false;
		return true;
	}

	/**
	 * How many mines can be put on map at the same time.
	 */
	public int getMaxMines() {
		return MAX_MINES;
	}
	

	/**
	 * Effect of mines. Their damaging and range of effect.
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
	public String toString() {
		return "MineGun [ID=" + ID + ", COST=" + COST + ", MAX_MINES=" + MAX_MINES + ", ZONES=" + Arrays.toString(ZONES)
				+ "]";
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + COST;
		result = prime * result + ID;
		result = prime * result + MAX_MINES;
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
		MineGun other = (MineGun) obj;
		return equals(other);
	}

}
