
package BaseLibrary.equipment;

import java.util.Arrays;

import BaseLibrary.communication.protocol.AProtocol;

public final class RepairTool implements IClassEquipment {

	private static final String NAME = "REPAIR_TOOL";

	static {
		AProtocol.registerForDeserialize(NAME, RepairTool.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	/**
	 * How many times per lap can be this robot tool used.
	 */
	public final int MAX_USAGES;

	/**
	 * Effect of reparation. Their hit point gain and range of effect.
	 */
	public final Zone[] ZONES;

	public final int ID;

	public final int COST;

	public RepairTool(int ID, int COST, int MAX_USAGES, Zone[] ZONES) {
		if (ZONES == null)
			throw new IllegalArgumentException("zones cannot be null");
		this.ID = ID;
		this.COST = COST;
		this.MAX_USAGES = MAX_USAGES;
		this.ZONES = new Zone[ZONES.length];
		for (int i = 0; i < ZONES.length; i++) {
			this.ZONES[i] = ZONES[i];
		}
	}

	/**
	 * How many times per lap can be this robot tool used.
	 */
	public final int getMaxUsages() {
		return MAX_USAGES;
	}

	/**
	 * Effect of reparation. Their hit point gain and range of effect.
	 */
	public final Zone[] getZones() {
		return ZONES;
	}

	@Override
	public int getCost() {
		return COST;
	}

	@Override
	public int getID() {
		return ID;
	}

	public boolean equals(RepairTool other) {
		if (COST != other.COST)
			return false;
		if (ID != other.ID)
			return false;
		if (MAX_USAGES != other.MAX_USAGES)
			return false;
		if (!Arrays.equals(ZONES, other.ZONES))
			return false;
		return true;
	}
	
	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + COST;
		result = prime * result + ID;
		result = prime * result + MAX_USAGES;
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
		RepairTool other = (RepairTool) obj;
		return equals(other);
	}

	@Override
	public String toString() {
		return "RepairTool [MAX_USAGES=" + MAX_USAGES + ", ZONES=" + Arrays.toString(ZONES) + ", ID=" + ID + ", COST="
				+ COST + "]";
	}

}
