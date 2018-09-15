
package BaseLibrary.communication.command.handshake;

import BaseLibrary.communication.protocol.AProtocol;

public class InitAnswerCommand extends AHandShakeCommand {

	
	private static final String NAME = "INIT_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, InitAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	
	private int maxTurn;
	private int lapNumber;
	private int maxLap;
	private int robotID;
	private int teamID;
	private int classEquipmentID;
	private int armorID;
	private int motorID;

	public InitAnswerCommand() {
	}

	public InitAnswerCommand(int maxTurn, int lapNumber, int maxLap, int robotID, int teamID, int classEquipmentID,
			int armorID, int motorID) {
		this.maxTurn = maxTurn;
		this.lapNumber = lapNumber;
		this.maxLap = maxLap;
		this.robotID = robotID;
		this.teamID = teamID;
		this.classEquipmentID = classEquipmentID;
		this.armorID = armorID;
		this.motorID = motorID;
		pending = false;
	}

	public void FillData(InitAnswerCommand source) {
		maxTurn = source.maxTurn;
		lapNumber = source.lapNumber;
		maxLap = source.maxLap;
		robotID = source.robotID;
		teamID = source.teamID;
		classEquipmentID = source.classEquipmentID;
		armorID = source.armorID;
		motorID = source.motorID;

		MORE = source.MORE;
		pending = false;
	}

	public int getMaxTurn() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return maxTurn;
	}

	public int getLapNumber() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return lapNumber;
	}

	public int getMaxLap() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return maxLap;
	}

	public int getRobotID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return robotID;
	}

	public int getTeamID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return teamID;
	}

	public int getClassEquipmentID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return classEquipmentID;
	}

	public int getArmorID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return armorID;
	}

	public int getMotorID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return motorID;
	}	
}
