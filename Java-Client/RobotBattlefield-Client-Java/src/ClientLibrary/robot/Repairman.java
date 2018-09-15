
package ClientLibrary.robot;

import java.util.concurrent.CompletableFuture;

import BaseLibrary.communication.command.repairman.RepairAnswerCommand;
import BaseLibrary.communication.command.repairman.RepairCommand;
import BaseLibrary.equipment.IClassEquipment;
import BaseLibrary.equipment.RepairTool;

/**
 * Instances represent robot who can repair other robots.
 */
public class Repairman extends ClientRobot {
	/**
	 * Repair tool witch robot has.
	 */
	private RepairTool repaitTool;

	/**
	 * Repair tool witch robot has.
	 */
	public RepairTool getRepaitTool() {
		return repaitTool;
	}

	public Repairman(String name) {
		super(name);
	}

	public Repairman(String name, String robotTeamName) {
		super(name, robotTeamName);
	}

	/**
	 * Repair robots in max range.
	 */
	public RepairAnswerCommand repair() {
		RepairAnswerCommand answer = new RepairAnswerCommand();
		addRobotTask(repairAsync(answer));
		return answer;
	}

	/**
	 * Repair robots closer then <code>maxDistance</code>.
	 * 
	 * @param maxDistance
	 *            - How far can be robots witch will be repaired.
	 */

	public RepairAnswerCommand repair(int maxDistance) {
		RepairAnswerCommand answer = new RepairAnswerCommand();
		addRobotTask(repairAsync(answer, maxDistance));
		return answer;
	}

	/**
	 * Repair robots in max range. And send it to server asynchronously.
	 * 
	 * @param destination
	 *            - Where to fill answer data.
	 * @param maxDistance
	 *            - How far can be robots witch will be repaired.
	 */

	private CompletableFuture<Void> repairAsync(RepairAnswerCommand destination) {
		return sendCommandAsync(new RepairCommand())
				.thenAcceptAsync((trash) -> receiveCommandAsync(RepairAnswerCommand.class)
						.thenAcceptAsync((answer) -> destination.FillData(answer)).join());

	}

	/**
	 * Repair robots closer then <code>maxDistance</code>. And send it to server
	 * asynchronously.
	 * 
	 * @param destination
	 *            - Where to fill answer data.
	 */

	private CompletableFuture<Void> repairAsync(RepairAnswerCommand destination, int maxDistance) {
		return sendCommandAsync(new RepairCommand(maxDistance))
				.thenAcceptAsync((trash) -> receiveCommandAsync(RepairAnswerCommand.class)
						.thenAcceptAsync((answer) -> destination.FillData(answer)).join());

	}

	@Override
	public RobotType getRobotType() {
		return RobotType.REPAIRMAN;
	}

	@Override
	protected void setClassEquip(int id) {
		repaitTool = REPAIR_TOOLS_BY_ID.get(id);
	}

	@Override
	protected IClassEquipment getClassEquip() {
		return repaitTool;
	}
}
