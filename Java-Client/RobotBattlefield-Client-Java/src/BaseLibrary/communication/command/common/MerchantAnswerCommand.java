
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

/**
 * Answer for merchant between turns.
 */
public class MerchantAnswerCommand extends ACommonCommand {

	private static final String NAME = "MERCHANT_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, MerchantAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	private int motorIdBought;

	/**
	 * What motor after buying robot has.
	 */
	public int getMotorIdBought() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return motorIdBought;
	}

	private int classEquipmentIdBought;

	/**
	 * What class equipment after buying robot has.
	 */
	public int getClassEquipmentIdBought() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return classEquipmentIdBought;
	}

	private int armorIdBought;

	/**
	 * What armor after buying robot has.
	 */
	public int getArmorIdBought() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return armorIdBought;
	}

	public MerchantAnswerCommand() {
	}

	public MerchantAnswerCommand(int motorIdBought, int armorIdBought, int classEquipmentIdBought) {
		this.motorIdBought = motorIdBought;
		this.armorIdBought = armorIdBought;
		this.classEquipmentIdBought = classEquipmentIdBought;
		pending = false;
	}

	public void FillData(MerchantAnswerCommand source) {
		this.motorIdBought = source.motorIdBought;
		this.armorIdBought = source.armorIdBought;
		this.classEquipmentIdBought = source.classEquipmentIdBought;
		this.MORE = source.MORE;
		pending = false;
	}
}
