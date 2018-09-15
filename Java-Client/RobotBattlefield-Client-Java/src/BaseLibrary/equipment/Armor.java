package BaseLibrary.equipment;

import BaseLibrary.communication.protocol.AProtocol;

public class Armor implements IEquipment {
	
	private static final String NAME = "ARMOR";

	static {
		AProtocol.registerForDeserialize(NAME, Armor.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	/**
	 * How taught is armor. How many hit points can robot have.
	 */
	public final int MAX_HP;

	public final int COST;

	public final int ID;


	public Armor(int ID, int COST, int MAX_HP) {
		this.MAX_HP = MAX_HP;
		this.COST = COST;
		this.ID = ID;
	}

	protected boolean equals(Armor other) {
		if (COST != other.COST)
			return false;
		if (ID != other.ID)
			return false;
		if (MAX_HP != other.MAX_HP)
			return false;
		return true;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + COST;
		result = prime * result + ID;
		result = prime * result + MAX_HP;
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
		Armor other = (Armor) obj;
		return equals(other);
	}
	@Override
	public int getCost() {
		return COST;
	}
	@Override
	public int getID() {
		return ID;
	}

	/**
	 * How taught is armor. How many hit points can robot have.
	 */
	public int getMaxHP() {
		return MAX_HP;
	}
}
