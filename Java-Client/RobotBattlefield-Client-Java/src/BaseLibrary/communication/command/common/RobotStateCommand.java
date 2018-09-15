
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

public class RobotStateCommand extends ACommonCommand {

	private static final String NAME = "STATE";

	static {
		AProtocol.registerForDeserialize(NAME, RobotStateCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	private double x;

	/**
	 * X-coordinate of robot position.
	 */
	public double getX() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return x;
	}

	private double y;

	/**
	 * Y-coordinate of robot position.
	 */
	public double getY() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return y;
	}

	private int hitPoints;

	/**
	 * Heal of robot.
	 */
	public int getHitPoints() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return hitPoints;
	}

	private double power;

	/**
	 * Actual robot's motor power.
	 */
	public double getPower() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return power;
	}

	private int turn;

	/**
	 * Actual turn.
	 */
	public int getTurn() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return turn;
	}

	private int maxTurn;

	/**
	 * Max turns in lap.
	 */
	public int getMaxTurn() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return maxTurn;
	}

	private int countOfLifeRobots;

	/**
	 * How many robot are life.
	 */
	public int getCountOfLifeRobots() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return countOfLifeRobots;
	}

	private int[] idsOfLifeRobots;

	/**
	 * IDs of living robot.
	 */
	public int[] getIdsOfLifeRobots() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return idsOfLifeRobots;
	}

	private EndLapCommand endLapCommand;

	public EndLapCommand getEndLapCommand() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return endLapCommand;
	}

	public RobotStateCommand(double x, double y, int hitPoints, double power, int turn, int maxTurn,
			int countOfLifeRobots, int[] idsOfLifeRobots, EndLapCommand endLapCommand) {
		this.x = x;
		this.y = y;
		this.hitPoints = hitPoints;
		this.power = power;
		this.turn = turn;
		this.maxTurn = maxTurn;
		this.countOfLifeRobots = countOfLifeRobots;
		this.idsOfLifeRobots = idsOfLifeRobots;
		this.endLapCommand = endLapCommand;

		pending = false;
	}

}
