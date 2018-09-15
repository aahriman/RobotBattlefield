
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

/**
 * Command for specific what robot want to buy.
 */
public class MerchantCommand extends ACommonCommand {

	private static final String NAME = "MERCHANT";

	static {
		AProtocol.registerForDeserialize(NAME, MerchantCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	private int motorID;

	/**
	 * What motor want to robot buy.
	 */
	public int getMotorID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return motorID;
	}

	private int classEquipmentID;

	/**
	 * What class equipment want to robot buy.
	 */
	public int getClassEquipmentID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return classEquipmentID;
	}

	private int armorID;

	/**
	 * What armor want to robot buy.
	 */
	public int getArmorID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return armorID;
	}

	private int repairHP;

	/**
	 * How many HP want robot to fix.
	 */
	public int getRepairHP() {
		return repairHP;
	}

	public MerchantCommand(int motorID, int armorID, int classEquipmentID, int repairHP) {
		this.motorID = motorID;
		this.classEquipmentID = classEquipmentID;
		this.armorID = armorID;
		this.repairHP = repairHP;
		pending = false;
	}
}
