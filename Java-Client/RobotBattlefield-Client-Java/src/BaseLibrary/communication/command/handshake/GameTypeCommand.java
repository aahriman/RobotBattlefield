package BaseLibrary.communication.command.handshake;

import BaseLibrary.communication.protocol.AProtocol;

public class GameTypeCommand extends AHandShakeCommand {

	private static final String NAME = "GAME_TYPE";

	static {
		AProtocol.registerForDeserialize(NAME, GameTypeCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	public static enum GameType {
		DEADMATCH, CAPTURE_FLAG, CAPTURE_BASE;
	}

	public final int ROBOTS_IN_ONE_TEAM;
	public final GameType GAME_TYPE;

	public GameTypeCommand(int ROBOTS_IN_ONE_TEAM, GameType GAME_TYPE) {
		this.ROBOTS_IN_ONE_TEAM = ROBOTS_IN_ONE_TEAM;
		this.GAME_TYPE = GAME_TYPE;
	}
}
