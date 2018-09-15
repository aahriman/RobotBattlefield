package BaseLibrary.equipment;

import BaseLibrary.communication.protocol.IInnerObject;

/**
 * Interface for specified that instance is equipment, can be bought during
 * merchant phase.
 */
public interface IEquipment extends IInnerObject{
	/**
	 * How many this equipment cost gold.
	 */
	public int getCost();

	/**
	 * This equipment's id for buying.
	 */
	public int getID();
}
