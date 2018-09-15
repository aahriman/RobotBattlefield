package BaseLibrary.communication.command.common;

import BaseLibrary.battlefield.LapState;
import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.IInnerObject;

/**
 * Command at the end of lap. This is sub command to RobotStateCommand
 */
public class EndLapCommand extends ACommonCommand implements IInnerObject {

	private static final String NAME = "END_LAP";

	static {
		AProtocol.registerForDeserialize(NAME, EndLapCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	private LapState state;

	/**
	 * Why lap end.
	 */
	public LapState getLapState() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return state;
	}

	private int gold;

	/**
	 * How many gold robot has.
	 */
	public int getGold() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return gold;
	}

	private int score;

	/**
	 * What robot score is.
	 */
	public int getScore() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return score;
	}

	public EndLapCommand(LapState state, int gold, int score) {
		this.state = state;
		this.gold = gold;
		this.score = score;
		pending = false;
	}
}
