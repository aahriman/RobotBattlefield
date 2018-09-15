package ClientLibrary.robot;

import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ExecutionException;

import BaseLibrary.battlefield.Robot;
import BaseLibrary.communication.NetworkStream;
import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.common.DriveAnswerCommand;
import BaseLibrary.communication.command.common.DriveCommand;
import BaseLibrary.communication.command.common.EndLapCommand;
import BaseLibrary.communication.command.common.MerchantAnswerCommand;
import BaseLibrary.communication.command.common.MerchantCommand;
import BaseLibrary.communication.command.common.RobotStateCommand;
import BaseLibrary.communication.command.common.ScanAnswerCommand;
import BaseLibrary.communication.command.common.ScanCommand;
import BaseLibrary.communication.command.common.WaitCommand;
import BaseLibrary.communication.command.equipment.GetArmorsAnswerCommand;
import BaseLibrary.communication.command.equipment.GetArmorsCommand;
import BaseLibrary.communication.command.equipment.GetGunsAnswerCommand;
import BaseLibrary.communication.command.equipment.GetGunsCommand;
import BaseLibrary.communication.command.equipment.GetMineGunsAnswerCommand;
import BaseLibrary.communication.command.equipment.GetMineGunsCommand;
import BaseLibrary.communication.command.equipment.GetMotorsAnswerCommand;
import BaseLibrary.communication.command.equipment.GetMotorsCommand;
import BaseLibrary.communication.command.equipment.GetRepairToolsAnswerCommand;
import BaseLibrary.communication.command.equipment.GetRepairToolsCommand;
import BaseLibrary.communication.command.handshake.ErrorCommand;
import BaseLibrary.communication.command.handshake.GameTypeCommand;
import BaseLibrary.communication.command.handshake.InitAnswerCommand;
import BaseLibrary.communication.command.handshake.InitCommand;
import BaseLibrary.config.GameProperties;
import BaseLibrary.equipment.Armor;
import BaseLibrary.equipment.Gun;
import BaseLibrary.equipment.IClassEquipment;
import BaseLibrary.equipment.MineGun;
import BaseLibrary.equipment.Motor;
import BaseLibrary.equipment.RepairTool;
import BaseLibrary.utils.AngleUtils;
import BaseLibrary.utils.ModUtils;
import ClientLibrary.ConnectionUtil;

/**
 * Abstract class for robot's classes at client side. Implement all common
 * methods.
 */
public abstract class ClientRobot extends Robot {
	/**
	 * Team name which is globally unique.
	 */
	public static final String TEAM_NAME = UUID.randomUUID().toString();

	private static final List<ClientRobot> ROBOT_COLLECTION = new ArrayList<ClientRobot>();
	private static final Map<ClientRobot, CompletableFuture<Void>> ROBOTS_TASK_COMMANDS = new HashMap<ClientRobot, CompletableFuture<Void>>();

	private static String ip = null;
	private static int port;

	// private static final String ERROR_FILE_NAME = "error.txt";

	static {
		ModUtils.loadMods();
	}

	/*
	 * if (!System.Diagnostics.Debugger.IsAttached) { // add handler for non
	 * debugging
	 * 
	 * uint numberOfTry = 0; while (true) { numberOfTry++; try { ERROR_FILE_NAME =
	 * errorName(numberOfTry); Console.SetError(new
	 * IndentedTextWriter(File.AppendText(ERROR_FILE_NAME))); break; } catch { //
	 * cannot open file, try next one if (numberOfTry > 1000) {
	 * Console.WriteLine("Cannot open error file. Application will be closed.");
	 * Thread.Sleep(1000); Environment.Exit(2); } } }
	 * 
	 * AppDomain currentDomain = AppDomain.CurrentDomain;
	 * currentDomain.UnhandledException += new
	 * UnhandledExceptionEventHandler(MyHandler);
	 * Thread.GetDomain().UnhandledException += new
	 * UnhandledExceptionEventHandler(MyHandler); } }
	 * 
	 * private static String errorName(uint numberOfTry) { return
	 * $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}-error-{numberOfTry}.txt";
	 * }
	 */

	/**
	 * Motors available to buy by its id.
	 */
	public static final HashMap<Integer, Motor> MOTORS_BY_ID = new HashMap<Integer, Motor>();

	/**
	 * Motors available to buy.
	 */
	public static final Collection<Motor> MOTORS = MOTORS_BY_ID.values();

	/**
	 * Armors available to buy by its id.
	 */
	public static final HashMap<Integer, Armor> ARMORS_BY_ID = new HashMap<Integer, Armor>();

	/**
	 * Armors available to buy.
	 */
	public static final Collection<Armor> ARMORS = ARMORS_BY_ID.values();

	/**
	 * Guns available to buy by its id.
	 */
	public static final HashMap<Integer, Gun> GUNS_BY_ID = new HashMap<Integer, Gun>();

	/**
	 * Guns available to buy.
	 */
	public static final Collection<Gun> GUNS = GUNS_BY_ID.values();

	/**
	 * Repair tools available to buy by its id.
	 */
	public static final HashMap<Integer, RepairTool> REPAIR_TOOLS_BY_ID = new HashMap<Integer, RepairTool>();

	/**
	 * Repair tools available to buy.
	 */
	public static final Collection<RepairTool> REPAIR_TOOLS = REPAIR_TOOLS_BY_ID.values();

	/**
	 * Mine guns available to buy by its id.
	 */
	public static final HashMap<Integer, MineGun> MINE_GUNS_BY_ID = new HashMap<Integer, MineGun>();

	/**
	 * Mine guns available to buy.
	 */
	public static final Collection<MineGun> MINE_GUNS = MINE_GUNS_BY_ID.values();

	/**
	 * Do connection to server.
	 * 
	 * @param args
	 *            <ul>
	 *            <li><description><code>args[0]</code> is IP address of
	 *            server.</description></li>
	 *            <li><description><code>args[1]</code> is port.</description></li>
	 *            <li><description>If <code>args.length &lt; 2</code> then default
	 *            port is choose, if <code>args.length &lt; 1</code> then local
	 *            adress is use as ip.</description></li>
	 *            </ul>
	 *            </param>
	 */
	public static GameTypeCommand connect(String[] args) {
		GameTypeCommand gameTypeCommand = null;
		synchronized (ROBOT_COLLECTION) {
			ip = ConnectionUtil.LOCAL_ADDRESS;
			port = GameProperties.DEFAULT_PORT;
			if (args.length >= 1) {
				ip = args[0];
			}

			if (args.length >= 2) {
				port = Integer.parseInt(args[1]);
			}
		}

		gameTypeCommand = getGameTypeAndLoadEquip();
		connectAllUnconnectedRobots();

		return gameTypeCommand;
	}

	private static GameTypeCommand getGameTypeAndLoadEquip() {
		ConnectionUtil connection = new ConnectionUtil();

		try {
			return connection.connectAsync(ip, port).thenApplyAsync((gameTypeCommand) -> {
				loadEquip(connection.COMMUNICATION);
				return gameTypeCommand;
			}).join();
		} catch (UnknownHostException e) {
			throw new RuntimeException(e.getMessage(), e);
		}
	}

	private static void connectAllUnconnectedRobots() {
		synchronized (ROBOT_COLLECTION) {
			for (ClientRobot robot : ROBOT_COLLECTION) {
				if (!robot.connected) {
					robot.connect(ip, port);
				}
			}
		}
	}

	/**
	 * End turn - every robots who do not send command will send command
	 * <code>Wait</code>.
	 * 
	 * @see doNothing
	 */
	public static void endTurn() {
		synchronized (ROBOTS_TASK_COMMANDS) {
			synchronized (ROBOT_COLLECTION) {
				for (ClientRobot robot : ROBOT_COLLECTION) {
					if (!ROBOTS_TASK_COMMANDS.containsKey(robot)) {
						robot.doNothing();
					}
				}
			}
		}
	}

	/**
	 * How many lap will play.
	 */
	public int MAX_LAP;

	/**
	 * Number of actual lap.
	 */
	public int LAP;

	/**
	 * How many turn is maximally in one lap.
	 */
	public int MAX_TURN;

	/**
	 * Number of actual turn.
	 */
	public int TURN;

	/**
	 * Communication with server.
	 */
	private NetworkStream sns;

	/**
	 * Robot's name.
	 */
	public String NAME;

	/**
	 * Robot's team name.
	 */
	public String ROBOT_TEAM_NAME;

	/**
	 * Flag if is robot already connected.
	 */
	private boolean connected;

	/**
	 * Create new instance of robot. Robot's team name is <code>TEAM_NAME</code>
	 * 
	 * @see TEAM_NAME
	 * @param name
	 *            - name of this robot
	 */
	protected ClientRobot(String name) {
		this(name, TEAM_NAME);
	}

	/**
	 * Create new instance of robot.
	 * 
	 * @param name
	 *            - name of this robot
	 * @param robotTeamName
	 *            - name of team
	 */
	protected ClientRobot(String name, String robotTeamName) {
		LAP = 1;
		this.NAME = name;
		this.ROBOT_TEAM_NAME = robotTeamName;

		synchronized (ROBOT_COLLECTION) {
			ROBOT_COLLECTION.add(this);

			if (ip != null) {
				connect(ip, port);
			}
		}
	}

	/**
	 * Connect unconnected robot.
	 */
	private void connect(String ip, int port) {
		if (!connected) {
			ConnectionUtil connection = new ConnectionUtil();

			try {
				connection.connectAsync(ip, port).thenAcceptAsync((trash) -> {
					sns = connection.COMMUNICATION;
					this.connected = true;
					initAsync(NAME, ROBOT_TEAM_NAME).join();
				}).join();

			} catch (UnknownHostException e) {
				throw new RuntimeException(e.getMessage(), e);
			}
		}
	}

	/**
	 * Set robot name and team name and send it to server asynchronously.
	 * 
	 * @param name
	 *            - name of this robot
	 * @param teamName
	 *            - name of team (suggest to use <code>ClientRobot.TEAM_NAME</code>)
	 */
	private CompletableFuture<InitAnswerCommand> initAsync(String name, String teamName) {
		return CompletableFuture.supplyAsync(() -> {
			sendCommandAsync(new InitCommand(name, teamName, getRobotType()));

			InitAnswerCommand answerCommand = (InitAnswerCommand) checkCommand(sns.receiveCommandAsync());
			processInit(answerCommand);

			RobotStateCommand robotState = (RobotStateCommand) checkCommand(sns.receiveCommandAsync());
			processState(robotState);
			return answerCommand;

		});
	}

	/**
	 * What kind of robot it is.
	 * 
	 * @seealso RobotType
	 */
	public abstract RobotType getRobotType();

	/**
	 * Get equipment from server.
	 */
	private static void loadEquip(NetworkStream sns) {
		synchronized (MOTORS_BY_ID) {
			if (MOTORS_BY_ID.size() == 0) {
				sns.sendCommand(new GetMotorsCommand());
				GetMotorsAnswerCommand motorAnswer = (GetMotorsAnswerCommand) checkCommand(sns.receiveCommand());

				sns.sendCommand(new GetArmorsCommand());
				GetArmorsAnswerCommand armorsAnswer = (GetArmorsAnswerCommand) checkCommand(sns.receiveCommand());

				sns.sendCommand(new GetGunsCommand());
				GetGunsAnswerCommand gunAnswer = (GetGunsAnswerCommand) checkCommand(sns.receiveCommand());

				sns.sendCommand(new GetRepairToolsCommand());
				GetRepairToolsAnswerCommand repairToolsAnswer = (GetRepairToolsAnswerCommand) checkCommand(
						sns.receiveCommand());

				sns.sendCommand(new GetMineGunsCommand());
				GetMineGunsAnswerCommand mineGunsAnswer = (GetMineGunsAnswerCommand) checkCommand(sns.receiveCommand());

				for (Motor motor : motorAnswer.getMotors()) {
					MOTORS_BY_ID.put(motor.ID, motor);
				}

				for (Armor armor : armorsAnswer.getArmors()) {
					ARMORS_BY_ID.put(armor.ID, armor);
				}

				for (Gun gun : gunAnswer.getGuns()) {
					GUNS_BY_ID.put(gun.ID, gun);
				}

				for (RepairTool repairTool : repairToolsAnswer.getRepairTools()) {
					REPAIR_TOOLS_BY_ID.put(repairTool.ID, repairTool);
				}

				for (MineGun mineGun : mineGunsAnswer.getMineGuns()) {
					MINE_GUNS_BY_ID.put(mineGun.ID, mineGun);
				}
			}
		}
	}

	/**
	 * Check if command is not ErrorCommand
	 * 
	 * @throws UnsupportedOperationException
	 *             - if command is ErrorCommand and text is message from
	 *             ErrorCommand
	 */
	private static <T extends ACommand> T checkCommand(T command) {
		if (command instanceof ErrorCommand) {
			ErrorCommand error = (ErrorCommand) command;
			throw new UnsupportedOperationException("ERROR:" + error.MESSAGE);
		}
		return command;
	}

	/**
	 * Check if command is not ErrorCommand
	 * 
	 * @throws ExecutionException
	 * @throws InterruptedException
	 * 
	 * @throws UnsupportedOperationException
	 *             - if command is ErrorCommand and text is message from
	 *             ErrorCommand
	 */
	private static <T extends ACommand> T checkCommand(CompletableFuture<T> futureCommand) {
		try {
			return checkCommand(futureCommand.get());
		} catch (InterruptedException | ExecutionException e) {
			throw new RuntimeException(e.getMessage(), e);
		}
	}

	/**
	 * Check if command is not ErrorCommand
	 * 
	 * @throws ExecutionException
	 * @throws InterruptedException
	 * 
	 * @throws UnsupportedOperationException
	 *             - if command is ErrorCommand and text is message from
	 *             ErrorCommand
	 */
	protected static <T extends ACommand> CompletableFuture<T> checkCommandAsync(CompletableFuture<T> futureCommand) {
		return futureCommand.thenApplyAsync((command) -> checkCommand(command));
	}

	/**
	 * Set robot id and equipment
	 */
	protected void processInit(InitAnswerCommand init) {
		this.ID = init.getRobotID();
		this.Motor = MOTORS_BY_ID.get(init.getMotorID());
		this.Armor = ARMORS_BY_ID.get(init.getRobotID());
		MAX_LAP = init.getMaxLap();
		TEAM_ID = init.getTeamID();
		MAX_TURN = init.getMaxTurn();
		setClassEquip(init.getClassEquipmentID());
	}

	/**
	 * Set class equipment to robot by id.
	 */
	protected abstract void setClassEquip(int id);

	/**
	 * What class equipment id is had by robot.
	 */
	protected abstract IClassEquipment getClassEquip();

	/**
	 * Set percentage power of motor and direction. At the end set
	 * <code>AngleDrive</code> if rotation success.
	 * 
	 * @param angle
	 *            - in degree. 0 = 3 hour. 90 = 6 hour and so on.
	 * @param power
	 *            - percentage from 0 to 100.
	 * @see Robot.AngleDrive
	 */
	public DriveAnswerCommand drive(double angle, double power) {
		DriveAnswerCommand answer = new DriveAnswerCommand();
		addRobotTask(driveAsync(answer, angle, power));
		return answer;
	}

	/**
	 * Set percentage power of motor and direction. It send this action to server
	 * asynchronously. At the end set <code>AngleDrive</code> if rotation success
	 * and fill answer data to <code>destination</code>.
	 * 
	 * @param destination
	 *            - where to fill answer data
	 * @param angle
	 *            - in degree. 0 = 3 hour. 90 = 6 hour and so on.
	 * @param power
	 *            - percentage from 0 to 100.
	 * @see Robot.AngleDrive
	 */
	private CompletableFuture<Void> driveAsync(DriveAnswerCommand destination, double angle, double power) {
		return sendCommandAsync(new DriveCommand(power, angle))
				.thenAcceptAsync((trash) -> receiveCommandAsync(DriveAnswerCommand.class).thenAcceptAsync((answer) -> {
					if (answer.isSuccess()) {
						AngleDrive = AngleUtils.normalizeDegree(angle);
					}
					destination.FillData(answer);
				}).join());
	}

	/**
	 * Robot make scan to angle and witch precision. That is mean if scanned robot
	 * is in angle to robot in range (angle - precision, angle+precision) it is
	 * success.
	 * 
	 * @param angle
	 *            - in degree. 0 = 3 hour. 90 = 6 hour and so on.
	 * @param precision
	 *            - parameter for sector. Min is 0 max is 10.
	 */
	public ScanAnswerCommand scan(double angle, double precision) {
		ScanAnswerCommand answer = new ScanAnswerCommand();
		addRobotTask(scanAsync(answer, angle, precision));
		return answer;
	}

	/**
	 * Robot make scan and send that action to server asynchronously. Answer data
	 * fill to <code>destination</code>.
	 * 
	 * @param destination
	 *            - where to fill answer data
	 * @param angle
	 *            - in degree. 0 = 3 hour. 90 = 6 hour and so on.
	 * @param precision
	 *            - parameter for sector. Min is 0 max is 10.
	 */
	private CompletableFuture<Void> scanAsync(ScanAnswerCommand destination, double angle, double precision) {
		return sendCommandAsync(new ScanCommand(precision, angle))
				.thenAcceptAsync((trash) -> receiveCommandAsync(ScanAnswerCommand.class)
						.thenAcceptAsync((answer) -> destination.FillData(answer)).join());

	}

	/**
	 * Set x,y coordinates, hit points and power. Set score and gold and call
	 * <code>sendMerchant</code> if is end of lap.
	 * 
	 * @seealso SendMerchant
	 */
	protected void processState(RobotStateCommand state) {
		this.X = state.getX();
		this.Y = state.getY();
		this.HitPoints = state.getHitPoints();
		this.Power = state.getPower();
		this.TURN = state.getTurn();
		EndLapCommand endLapCommand = state.getEndLapCommand();
		if (endLapCommand != null) {
			Gold = endLapCommand.getGold();
			Score = endLapCommand.getScore();

			if (LAP == MAX_LAP) {
				System.out.println("Match finish.");
				System.exit(0);
			}
			sendMerchant();
			LAP++;
		}
	}

	/**
	 * Robot will do nothing for this turn, but still move.
	 */
	public void doNothing() {
		addRobotTask(doNothingAsync());
	}

	/**
	 * Robot set action to do nothing and sent it to server asynchronously.
	 */
	private CompletableFuture<Void> doNothingAsync() {
		return sendCommandAsync(new WaitCommand()).thenAcceptAsync(
				(trash) -> processState((RobotStateCommand) checkCommandAsync(sns.receiveCommandAsync()).join()));
	}

	/**
	 * Send request for buying motorID, armorID, classEquipmentId and
	 * repairHitPoints.
	 */
	protected void merchant(int motorID, int armorID, int classEquipmentID, int repairHitPoints) {
		addRobotTask(merchantAsync(motorID, armorID, classEquipmentID, repairHitPoints));
	}

	/**
	 * Asynchronously send request for buying motorID, armorID, classEquipmentId and
	 * repairHitPoints.
	 */
	private CompletableFuture<Void> merchantAsync(int motorID, int armorID, int classEquipmentID, int repairHitPoints) {
		return sendCommandAsync(new MerchantCommand(motorID, armorID, classEquipmentID, repairHitPoints))
				.thenAccept((trash) -> receiveCommandAsync(MerchantAnswerCommand.class)
						.thenAccept((answer) -> processMerchant(answer)).join());

	}

	/**
	 * Setting what you want to buy at the end of lap. Use <code>Merchant</code>
	 * method for buying.
	 * 
	 * @see merchant
	 */
	protected void sendMerchant() {
		merchant(Motor.ID, Armor.ID, getClassEquip().getID(), HitPoints - Armor.MAX_HP);
	}

	/**
	 * Processing merchant answer. Set motor, armor and class equipment.
	 */
	protected void processMerchant(MerchantAnswerCommand merchantAnswer) {
		this.Motor = MOTORS_BY_ID.get(merchantAnswer.getMotorIdBought());
		this.Armor = ARMORS_BY_ID.get(merchantAnswer.getArmorIdBought());
		setClassEquip(merchantAnswer.getClassEquipmentIdBought());
	}

	/**
	 * Asynchronously sending command.
	 * 
	 * @param command
	 *            - Instance of command witch you want to send.
	 */
	protected CompletableFuture<Void> sendCommandAsync(ACommand command) {
		return sns.sendCommandAsync(command);
	}

	/**
	 * Asynchronously receiving command from server.
	 */
	@SuppressWarnings("unchecked")
	protected <T extends ACommand> CompletableFuture<T> receiveCommandAsync(Class<T> clazz) {
		return checkCommandAsync(sns.receiveCommandAsync())
				.thenApplyAsync((command) -> checkCommandAsync(sns.receiveCommandAsync()).thenApplyAsync((state) -> {
					processState((RobotStateCommand) state);
					return (T) command;
				}).join());
	}

	/**
	 * Receiving command from server.
	 */
	protected <T extends ACommand> T receiveCommand(Class<T> clazz) {
		T command;
		try {
			command = (T) receiveCommandAsync(clazz).get();
			return checkCommand(command);
		} catch (InterruptedException | ExecutionException e) {
			throw new RuntimeException(e.getMessage(), e);
		}
	}

	/**
	 * Add robot task, so ClientRobot class know when all robot have prepared
	 * command.
	 * 
	 * @throws UnsupportedOperationException
	 *             - when robot is not connected or when robot already select action
	 *             for this turn.
	 */

	protected void addRobotTask(CompletableFuture<Void> t) {
		if (!connected) {
			throw new UnsupportedOperationException(
					"Robots have to be connected. Use ClientRobot.Connect(args) first.");
		}

		synchronized (ROBOTS_TASK_COMMANDS) {
			if (ROBOTS_TASK_COMMANDS.containsKey(this)) {
				throw new UnsupportedOperationException("Robot can not send more then one command during same turn.");
			}
			ROBOTS_TASK_COMMANDS.put(this, t);
			synchronized (ROBOT_COLLECTION) {
				if (ROBOTS_TASK_COMMANDS.size() == ROBOT_COLLECTION.size()) {
					for (CompletableFuture<Void> task : ROBOTS_TASK_COMMANDS.values()) {
						task.join();
					}
					ROBOTS_TASK_COMMANDS.clear();
				}
			}
		}
	}

	/*
	 * static void MyHandler(object sender, UnhandledExceptionEventArgs e) {
	 * Console.Error.WriteLine(DateTime.Now);
	 * Console.Error.WriteLine(e.ExceptionObject); Console.Error.Flush(); if
	 * (e.ExceptionObject instanceof Exception ex) { Console.WriteLine("ERROR:'" +
	 * ex.Message + "'. Application store more information in " + ERROR_FILE_NAME +
	 * " and will be closed."); } else {
	 * Console.WriteLine("Some error occurs. Application store more information in "
	 * + ERROR_FILE_NAME + " and will be closed."); }
	 * Console.WriteLine("Continue with pressing eny key."); Console.Read();
	 * Environment.Exit(1); }
	 */
}