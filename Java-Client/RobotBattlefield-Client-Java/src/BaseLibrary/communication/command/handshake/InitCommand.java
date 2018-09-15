
package BaseLibrary.communication.command.handshake;

import BaseLibrary.battlefield.Robot.RobotType;
import BaseLibrary.communication.protocol.AProtocol;

public class InitCommand extends AHandShakeCommand {
	
	private static final String NAME = "INIT";

	static {
		AProtocol.registerForDeserialize(NAME, InitCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	public static int NAME_MAX_length = 10;
	public static int TEAM_NAME_MAX_length = 40;

	private String name;
	private String teamName;
	private RobotType robotType;

	public InitCommand(String name) {
		this(name, null);
	}

	public InitCommand(String name, String teamName) {
		this(name, teamName, RobotType.TANK);
	}

	public InitCommand(String name, String teamName, RobotType robotType) {
		if (name == null) {
			throw new IllegalArgumentException("name cannot be null.");
		}

		if (teamName == null) {
			teamName = "";
		}

		this.name = name.substring(0, Math.min(name.length(), NAME_MAX_length));
		this.teamName = teamName.substring(0, Math.min(teamName.length(), TEAM_NAME_MAX_length));
		this.robotType = robotType;
		pending = false;
	}

	public String getName() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return name;
	}
	public String getTeamName() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return teamName;
	}
	public RobotType getRobotType() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return robotType;
	}
}
