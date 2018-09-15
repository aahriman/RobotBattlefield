package ClientLibrary.robot;

import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.CompletableFuture;

import BaseLibrary.communication.command.miner.DetonateMineAnswerCommand;
import BaseLibrary.communication.command.miner.DetonateMineCommand;
import BaseLibrary.communication.command.miner.PutMineAnswerCommand;
import BaseLibrary.communication.command.miner.PutMineCommand;
import BaseLibrary.equipment.IClassEquipment;
import BaseLibrary.equipment.MineGun;
import BaseLibrary.utils.euclidianSpaceStruct.Point;

/**
 * Instances represent robot who can put mines on map.
 */
public class MineLayer extends ClientRobot {

	/**
	 * Structure for information about mine.
	 */
	public final class Mine {

		/**
		 * Mine id witch is useful for detonation.
		 */
		public final int ID;

		/**
		 * X coordinate on map.
		 */
		public final double X;

		/**
		 * Y coordinate on map.
		 */
		public final double Y;

		/**
		 * Mine position.
		 */
		public Point getPosition() {
			return new Point(X, Y);
		}

		/**
		 * Create mine with defined ID.
		 */

		public Mine(int id, double x, double y) {
			ID = id;
			X = x;
			Y = y;
		}
	}

	private MineGun mineGun;

	/**
	 * Mine gun witch robot has.
	 */
	public MineGun getMineGun() {
		return mineGun;
	}

	/**
	 * Number of put mines on map.
	 */
	private int putMines;

	public int getPutMines() {
		return putMines;
	}

	/**
	 * List of available mines put on map.
	 */
	public final List<Mine> PUT_MINES_LIST = new LinkedList<Mine>();

	/**
	 * Create new mineLayer with name.
	 */

	public MineLayer(String name) {
		super(name);
	}

	/**
	 * Create new mineLayer with name and into team specified by teamName
	 */

	public MineLayer(String name, String robotTeamName) {
		super(name, robotTeamName);
	}

	/**
	 * Put mine on robot position.
	 */
	public PutMineAnswerCommand putMine() {
		PutMineAnswerCommand answer = new PutMineAnswerCommand();
		addRobotTask(putMineAsync(answer));
		return answer;
	}

	/**
	 * Put mine on robot position.
	 * 
	 * @param destination
	 *            - Where to fill answer data.
	 */

	private CompletableFuture<Void> putMineAsync(PutMineAnswerCommand destination) {

		Point position = getPosition();

		return sendCommandAsync(new PutMineCommand()).thenAcceptAsync(
				(trash) -> receiveCommandAsync(PutMineAnswerCommand.class).thenAcceptAsync((answer) -> {
					destination.FillData(answer);
					if (answer.isSuccess()) {
						PUT_MINES_LIST.add(new Mine(answer.getMineID(), position.X, position.Y));
						putMines++;
					}
				}).join());
	}

	/**
	 * Detonate specified mine.
	 * 
	 * @param mineID
	 *            - witch mine robot wants to detonate.
	 */

	public DetonateMineAnswerCommand detonateMine(int mineID) {
		DetonateMineAnswerCommand answer = new DetonateMineAnswerCommand();
		addRobotTask(detonateMineAsync(answer, mineID));
		return answer;
	}

	/**
	 * Detonate specified mine. Send action to server asynchronously.
	 * 
	 * @param destination
	 *            - Where to fill answer data.
	 * @param mineID
	 *            - witch mine robot wants to detonate.
	 */
	private CompletableFuture<Void> detonateMineAsync(DetonateMineAnswerCommand destination, int mineID) {
		return sendCommandAsync(new DetonateMineCommand(mineID)).thenAcceptAsync(
				(trash) -> receiveCommandAsync(DetonateMineAnswerCommand.class).thenAcceptAsync((answer) -> {
					destination.FillData(answer);

					if (answer.isSuccess()) {
						putMines--;

						for (Iterator<Mine> it = PUT_MINES_LIST.iterator(); it.hasNext();) {
							Mine mine = it.next();
							if (mine.ID == mineID) {
								it.remove();
								break;
							}
						}
					}
				}).join());
	}

	@Override
	public RobotType getRobotType() {
		return RobotType.MINE_LAYER;
	}

	@Override
	protected void setClassEquip(int id) {
		mineGun = MINE_GUNS_BY_ID.get(id);
	}

	@Override
	protected IClassEquipment getClassEquip() {
		return mineGun;
	}
}
