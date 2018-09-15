
package ClientLibrary.robot;

import java.util.concurrent.CompletableFuture;

import BaseLibrary.communication.command.tank.ShootAnswerCommand;
import BaseLibrary.communication.command.tank.ShootCommand;
import BaseLibrary.equipment.Gun;
import BaseLibrary.equipment.IClassEquipment;

/**
 * Instances represent robot who can shoot bullets.
 */
public class Tank extends ClientRobot {

	/**
	 * Gun witch robot has.
	 */
	private Gun gun;

	/**
	 * Gun witch robot has.
	 */
	public Gun getGun() {
		return gun;
	}

	public Tank(String name) {
		super(name);
	}

	public Tank(String name, String robotTeamName) {
		super(name, robotTeamName);
	}

	/**
	 * Shoot bullet.
	 * 
	 * @param angle
	 *            - in degree. 0 = 3 hour. 90 = 6 hour and so on.
	 * @param range
	 *            - how far this robot wants to shot
	 */
	public ShootAnswerCommand shoot(double angle, double range) {
		ShootAnswerCommand answer = new ShootAnswerCommand();
		addRobotTask(shootAsync(answer, angle, range));
		return answer;
	}

	/**
	 * Shoot bullet. And sent it to server asynchronously.
	 * 
	 * @param destination
	 *            - Where to fill answer data.
	 * @param angle
	 *            - in degree. 0 = 3 hour. 90 = 6 hour and so on.
	 * @param range
	 *            - how far this robot wants to shot
	 */
	private CompletableFuture<Void> shootAsync(ShootAnswerCommand destination, double angle, double range) {
		return sendCommandAsync(new ShootCommand(range, angle))
				.thenAcceptAsync((trash) -> receiveCommandAsync(ShootAnswerCommand.class)
						.thenAcceptAsync((answer) -> destination.FillData(answer)).join());
	}
	
	

	@Override
	public RobotType getRobotType() {
		return RobotType.TANK;
	}

	@Override
	protected void setClassEquip(int id) {
		gun = GUNS_BY_ID.get(id);
	}

	@Override
	protected IClassEquipment getClassEquip() {
		return gun;
	}
}
