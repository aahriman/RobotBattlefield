package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.equipment.MineGun;

public class GetMineGunsAnswerCommand extends AEquipmentCommand {

	private static final String NAME = "MINE_GUNS_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, GetMineGunsAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private MineGun[] mineGuns;

	public GetMineGunsAnswerCommand(MineGun[] mineGuns) {
		setMineGuns(mineGuns);
	}

	/**
	 * Available mine guns to buy.
	 * 
	 * @see MineGun
	 */
	public MineGun[] getMineGuns() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return mineGuns;
	}

	private void setMineGuns(MineGun[] mineGuns) {
		pending = false;
		this.mineGuns = mineGuns;
	}
}
