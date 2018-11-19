using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.communication;
using BaseLibrary.communication.command;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.communication.protocol;
using BaseLibrary.equipment;
using BaseLibrary.utils;
using BattlefieldLibrary.battlefield.robot;
using BattlefieldLibrary.config;
using BattlefieldLibrary.gui;
using ObstacleMod;
using ViewerLibrary;
using ViewerLibrary.model;
using ViewerLibrary.serializer;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;
using Point = BaseLibrary.utils.euclidianSpaceStruct.Point;


namespace BattlefieldLibrary.battlefield {
	public abstract partial class Battlefield {

        static Battlefield() {
            ModUtils.LoadMods();
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ObstaclesAroundRobot).TypeHandle);
        }

	    /// <summary>
	    /// Store robot instance and battlefield instance
	    /// </summary>
        public struct RobotAndBattlefield {
	        public readonly BattlefieldRobot ROBOT;
	        public readonly Battlefield BATTLEFIELD;

	        public RobotAndBattlefield(BattlefieldRobot robot, Battlefield battlefield) {
	            ROBOT = robot;
	            BATTLEFIELD = battlefield;
	        }
        }

        /// <summary>
        /// Store network stream and battlefield
        /// </summary>
	    public struct NetworkStreamAndBattlefield {
            /// <summary>
            /// Network stream for communication.
            /// </summary>
	        public readonly NetworkStream NETWORK_STREAM;
            
	        public readonly Battlefield BATTLEFIELD;

	        public NetworkStreamAndBattlefield(NetworkStream networkStream, Battlefield battlefield) {
	            NETWORK_STREAM = networkStream;
	            BATTLEFIELD = battlefield;
	        }
	    }

        /// <summary>
        /// Config for this battlefield.
        /// </summary>
	    public readonly BattlefieldConfig CONFIG;

        /// <summary>
        /// Max size of arena. Arena is from [0, 0] to [ARENA_MAX_SIZE, ARENA_MAX_SIZE].
        /// </summary>
        public const int ARENA_MAX_SIZE = 1000;


        /// <summary>
        /// Battlefield states.
        /// </summary>
        public enum BattlefieldState {
            /// <summary>
            /// For initialization. Get equipments commands and init command.
            /// </summary>
	        GETTING_EQUIPMENT,
            /// <summary>
            /// One lap is running. Robots are moving and shooting and so on.
            /// </summary>
            FIGHT,
            /// <summary>
            /// Time to buy equipment and repair.
            /// </summary>
            MERCHANT
	    }

        /// <summary>
        /// For store more data.
        /// </summary>
	    public Dictionary<String, Object> MORE_VARIABLES = new Dictionary<string, object>();

        /// <summary>
        /// Actual lap number.
        /// </summary>
		protected int lap = 1;
	    
        /// <summary>
        /// Motor equipment available for this battle.
        /// </summary>
		public Motor[] Motors { get; private set; }

	    /// <summary>
	    /// Gun equipment available for this battle.
	    /// </summary>
		public Gun[] Guns { get; private set; }

	    /// <summary>
	    /// Armor equipment available for this battle.
	    /// </summary>
		public Armor[] Armors { get; private set; }

	    /// <summary>
	    /// Mine gun equipment available for this battle.
	    /// </summary>
        public MineGun[] MineGuns { get; private set; }

	    /// <summary>
	    /// Repairs tools equipment available for this battle.
	    /// </summary>
        public RepairTool[] RepairTools { get; private set; }

        /// <summary>
        /// List of robots witch participate in battle.
        /// </summary>
        protected List<BattlefieldRobot> robots = new List<BattlefieldRobot>();

	    /// <summary>
	    /// List of robots witch was killed in actual turn.
	    /// </summary>
	    protected List<BattlefieldRobot> killedRobots = new List<BattlefieldRobot>();

        /// <summary>
        /// Robots split by its id.
        /// </summary>
	    protected Dictionary<int, BattlefieldRobot> robotsById = new Dictionary<int, BattlefieldRobot>();

        /// <summary>
        /// Team ids split by its names.
        /// </summary>
        protected Dictionary<string, int> robotTeamIdByTeamName = new Dictionary<string, int>();

        /// <summary>
        /// List of robot in one team by team id.
        /// </summary>
        protected Dictionary<int, List<BattlefieldRobot>> robotsByTeamId = new Dictionary<int, List<BattlefieldRobot>>();

        /// <summary>
        /// Manager for obstacles.
        /// </summary>
        protected ObstacleManager obstacleManager;

        /// <summary>
        /// Counter for generate id.
        /// </summary>
        private int idForRobot = 1;

	    /// <summary>
	    /// Counter for generate id for team.
	    /// </summary>
	    private int idForTeam = 1;

	    /// <summary>
        /// How long wait between turn (negative is forever) in millisecond.
        /// </summary>
	    private readonly int WAITING_TIME_BETWEEN_TURNS;

        /// <summary>
        /// How long robot can miss communication (<code>8*WAITING_TIME_BETWEEN_TURNS</code>)
        /// </summary>
	    private readonly TimeSpan MAX_WAITING_TIME;

        protected bool _run = false;

        /// <summary>
        /// Is battle running or set battle to run.
        /// </summary>
		public bool RUN {
			get => _run;
		    set => setRun(value);
		}

        /// <summary>
        /// This mutex is set when arena start.
        /// </summary>
        public readonly ManualResetEvent RunEvent = new ManualResetEvent(false);

        /// <summary>
        /// This thread is only for wait until simulation will end. It start when all robots are connected.
        /// </summary>
        public readonly Thread RunThread;

        /// <summary>
        /// Commands splits by robot.
        /// </summary>
		private readonly Dictionary<BattlefieldRobot, ACommand> receivedCommands = new Dictionary<BattlefieldRobot, ACommand>();

        /// <summary>
        /// List of bullet with explode by turn when explode.
        /// </summary>
		private readonly IDictionary<int, List<Bullet>> heapBullet = new Dictionary<int, List<Bullet>>();

	    /// <summary>
	    /// List of tanks witch reload gun bu turn when reloaded.
	    /// </summary>
        private readonly IDictionary<int, List<Tank>> gunLoaded = new Dictionary<int, List<Tank>>();

        /// <summary>
        /// List of mines witch detonate in actual turn.
        /// </summary>
        private readonly IList<Mine> detonatedMines = new List<Mine>();

        /// <summary>
        /// TaskCompletion for async waiting. It is finish when every robot send command.
        /// </summary>
		private TaskCompletionSource<bool> allSendCommand = new TaskCompletionSource<bool>();

        /// <summary>
        /// Instance of merchant for buying equipment.
        /// </summary>
		protected Merchant merchant;

        /// <summary>
        /// Actual turn.
        /// </summary>
		protected int turn { get; private set; }

        /// <summary>
        /// How many teams will play this battle.
        /// </summary>
	    public readonly int TEAMS;

        /// <summary>
        /// How many robots in one team in this battle.
        /// </summary>
	    public readonly int ROBOTS_IN_TEAM;

        /// <summary>
        /// How many turn at max in one lap.
        /// </summary>
	    public readonly int MAX_TURN;

        /// <summary>
        /// How many laps per battle.
        /// </summary>
	    public readonly int MAX_LAP;

        /// <summary>
        /// How long it takes to respawn robot.
        /// </summary>
	    public readonly int RESPAWN_TIMEOUT = 0;

        /// <summary>
        /// Is re-spawning allowed.
        /// </summary>
	    private readonly bool RESPAWN_ALLOWED;

        /// <summary>
        /// Instance for storing what happen in this turn.
        /// </summary>
        /// <seealso cref="SERIALIZER"/>
        protected BattlefieldTurn battlefieldTurn = new BattlefieldTurn(0);

        /// <summary>
        /// Turn serializator, after every turn match is serialized to file for later record.
        /// </summary>
        private readonly JSONSerializer SERIALIZER = new JSONSerializer();

        /// <summary>
        /// Cache for alive robots in turn.
        /// </summary>
        private readonly Cache<int?, List<BattlefieldRobot>> cache = new Cache<int?, List<BattlefieldRobot>>(true);

        /// <summary>
        /// Actual battle state
        /// </summary>
        BattlefieldState _battlefieldState = BattlefieldState.GETTING_EQUIPMENT;

	    /// <summary>
	    /// Processor for commands in state GETTING_EQUIPMENT
	    /// </summary>
	    protected CommandProcessor<ACommand, NetworkStreamAndBattlefield> commandProcessorBeforeInitRobot;
        /// <summary>
        /// Processor for commands
        /// </summary>
        protected CommandProcessor<ACommand, RobotAndBattlefield> commandProcessor;

	    private StreamWriter writer;

        private readonly Dictionary<int, List<BattlefieldRobot>> RESPAWN_ROBOT_AT_TURN = new Dictionary<int, List<BattlefieldRobot>>();

        /// <summary>
        /// Data model for drawing battle on-line.
        /// </summary>
	    private readonly SerialTurnDataModel turnDataModel;


        private readonly Dictionary<NetworkStream, BattlefieldRobot> robotsByStream = new Dictionary<NetworkStream, BattlefieldRobot>();

	    public static void addDamagingMethod(DamageDealing damageDealing) {
	        damageDealingMethods.Add(damageDealing);
	    }

	    private static readonly List<DamageDealing> damageDealingMethods = new List<DamageDealing>();


        private BattlefieldViewer viewer;

        protected Battlefield(BattlefieldConfig battlefieldConfig) {
            ROBOTS_IN_TEAM = battlefieldConfig.ROBOTS_IN_TEAM;
            TEAMS = battlefieldConfig.TEAMS;
            MAX_LAP = battlefieldConfig.MAX_LAP;
            MAX_TURN = battlefieldConfig.MAX_TURN;
            RESPAWN_TIMEOUT = battlefieldConfig.RESPAWN_TIMEOUT;
            RESPAWN_ALLOWED = battlefieldConfig.RESPAWN_ALLOWED;
            WAITING_TIME_BETWEEN_TURNS = battlefieldConfig.WAITING_TIME_BETWEEN_TURNS;
            MAX_WAITING_TIME = new TimeSpan(0, 0, 0, 0, 8 * WAITING_TIME_BETWEEN_TURNS); //How long wait for receive command before disconnect
            CONFIG = battlefieldConfig;

            if (battlefieldConfig.GUI) {
                turnDataModel = new SerialTurnDataModel();
                //viewer = new BattlefieldViewer(turnDataModel);

                Thread t = new Thread((battlefield) => {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    viewer = new BattlefieldViewer(turnDataModel);
                    Application.Run(viewer);
                });
                t.Start(this);
            }

            if (battlefieldConfig.EQUIPMENT_CONFIG_FILE != null) {
	            EqipmentConfig.SetEquipmentFromFile(battlefieldConfig.EQUIPMENT_CONFIG_FILE);
	        } else {
                EqipmentConfig.SetDefaultEquipment();
	        }
            IEnumerable<IObstacle> obstacles = (battlefieldConfig.OBSTACLE_CONFIG_FILE != null) ? (IEnumerable<IObstacle>) ObstacleManager.LoadObstaclesFromFile(battlefieldConfig.OBSTACLE_CONFIG_FILE) : new IObstacle[0];
	        obstacleManager = new ObstacleManager(obstacles, battlefieldConfig.RANDOM_SEED);
	       
	        battlefieldSetting(EqipmentConfig.MOTORS, EqipmentConfig.GUNS, EqipmentConfig.ARMORS, EqipmentConfig.REPAIR_TOOLS, EqipmentConfig.MINE_GUNS, battlefieldConfig.MATCH_SAVE_FILE);

            RunThread = new Thread(running);

           commandProcessorBeforeInitRobot = new CommandProcessor<ACommand, NetworkStreamAndBattlefield>(command => new ErrorCommand("Unsupported command " + command.GetType().Name + ". Arena is in " + _battlefieldState + "."));
           commandProcessor = new CommandProcessor<ACommand, RobotAndBattlefield>(command => new ErrorCommand("Unsupported command " + command.GetType().Name + ". Arena is in " + _battlefieldState + "."));
           addProcess();     
        }

        private void battlefieldSetting(Motor[] motors, Gun[] guns, Armor[] armors, RepairTool[] repairTools, MineGun[] mineGuns, string filename) {
            this.Motors = motors;
			this.Guns = guns;
			this.Armors = armors;
            this.MineGuns = mineGuns;
            this.RepairTools = repairTools;

            
			merchant = new Merchant(motors, armors, guns, repairTools, mineGuns);

            if (File.Exists(filename)) {
                File.Delete(filename);
            }

            writer = new StreamWriter(File.OpenWrite(filename)) {AutoFlush = true};
        }

	    private bool _end = false;
        public bool End() {
	        return _end;
	    }

		public void Start() {
			setRun(true);
		}

        protected async void listen(NetworkStream sns) {
			await Task.Yield();
            try {
                ACommand command = await sns.ReceiveCommandAsync();

                switch (command) {
                    case AEquipmentCommand _:
                        await sns.SendCommandAsync(commandProcessorBeforeInitRobot.Process(command, new NetworkStreamAndBattlefield(sns, this)));
                        listen(sns);
                        break;
                    case InitCommand _:
                        ACommand answerCommand = commandProcessorBeforeInitRobot.Process(command, new NetworkStreamAndBattlefield(sns, this));
                        await sns.SendCommandAsync(answerCommand);
                        if (!(answerCommand is ErrorCommand)) {
                            await sendRobotStateCommandAsync(LapState.NONE, robotsByStream[sns]);
                            lock (robots) {

                                if (robots.Count == TEAMS * ROBOTS_IN_TEAM) {
                                    setRun(true);
                                }
                            }

                            listen(sns);
                        }
                        break;
                    default:
                        lock (receivedCommands) {
                            receivedCommands.Add(robotsByStream[sns], command);
                            lock (robots) {
                                if (_run && robots.Count == receivedCommands.Count) {
                                    allSendCommand.TrySetResult(true);
                                }
                            }
                        }
                        break;
                }
                return;
            } catch (Exception e) when(e is IOException || e is ObjectDisposedException || e is ArgumentException) {
                // IOException | ObjectDisposedException - client was disconnected
                lock (robots) {
                    if (robotsByStream.ContainsKey(sns)) {
                        BattlefieldRobot robot = robotsByStream[sns];
                        robots.Remove(robotsByStream[sns]);
                        Console.WriteLine("Disconnected robot " + robot.NAME + " because " + e.Message);
                        sns.Close();
                    } else if (e is ArgumentException) {
                        Console.WriteLine(e.Message);
                    }
                }
                lock (receivedCommands) {
                    lock (robots) {
                        if (_run && robots.Count == receivedCommands.Count) {
                            allSendCommand.TrySetResult(true);
                        }
                    }
                }
                
            }
		}

		public bool AddRobot(NetworkStream n) {
			bool ret = false;
		    
		    if (!RUN && (TEAMS * ROBOTS_IN_TEAM <= 0 || robots.Count < TEAMS * ROBOTS_IN_TEAM)) {
                listen(n);
		        ret = true;
		    }
		    
		    return ret;
		}

		protected void setRun(bool run) {
			if (_run == run) {
				return;
			}
			if(_run){
				throw new ArgumentException("When once arena run, it can not be stop.");
			}

			_run = run;
			if (_run) {
			    viewer?.EnableStart();
				firstBattle();
				RunThread.Start();
			    RunEvent.Set();
			}
		}

		public bool isRun() {
			return RUN;
		}

        protected void newBattle() {
			turn = 0;
		    lap++;
			foreach (BattlefieldRobot r in robots) {
				r.Power = 0;
				r.WantedPower = 0;
			    Point position = obstacleManager.StartRobotPosition(ARENA_MAX_SIZE, ARENA_MAX_SIZE);

                r.X = position.X;
				r.Y = position.Y;

                Tank tank = r as Tank;
                if (tank != null) {
                    tank.GunsToLoad = 0;
                }

                MineLayer mineLayer = r as MineLayer;
                if (mineLayer != null) {
                    mineLayer.MinesNow = 0;
                    mineLayer.MINES_BY_ID.Clear();
                }

                Repairman repairman = r as Repairman;
                if (repairman != null) {
                    repairman.RepairToolUsed = 0;
                }
            }
            heapBullet.Clear();
            detonatedMines.Clear();
		}

		protected virtual void firstBattle() {
            Console.WriteLine("Battle start");
		    newBattle();
		    lap = 1;
		}

		protected List<BattlefieldRobot> getAliveRobots() {
			if (cache.IsCached(turn)) {
				return cache.GetCached();
			}
			var aliveRobots = new List<BattlefieldRobot>();
			foreach (BattlefieldRobot r in robots) {
				if (r.HitPoints > 0) {
					aliveRobots.Add(r);
				}
			}
			cache.Cached(turn, aliveRobots);
			return aliveRobots;
		}

	    protected virtual void handleEndTurn(bool last) {
	        Turn turn;
            lock (writer) {
	            foreach (BattlefieldRobot r in robots) {
	                battlefieldTurn.AddRobot(new ViewerLibrary.Robot(r.ID, r.TEAM_ID, r.Score, r.Gold, r.HitPoints, r.X, r.Y,
	                                                                     r.AngleDrive, r.NAME));
	            }

                foreach (var pairValueKeyBullets in heapBullet) { // add bullets
                    foreach (var bullet in pairValueKeyBullets.Value) {
                        int difference = this.turn - bullet.FROM_LAP;
                        double speedX = (bullet.TO_X - bullet.FROM_X) / (bullet.TO_LAP - bullet.FROM_LAP);
                        double speedY = (bullet.TO_Y - bullet.FROM_Y) / (bullet.TO_LAP - bullet.FROM_LAP);
                        double x = bullet.FROM_X + difference * speedX;
                        double y = bullet.FROM_Y + difference * speedY;
                        battlefieldTurn.AddBullet(new ViewerLibrary.Bullet(x, y, pairValueKeyBullets.Key == this.turn));
                    }
                }

                foreach (var robot in robots) { // add not detonated mines
                    switch (robot.ROBOT_TYPE) {
                        case RobotType.MINE_LAYER:
                            MineLayer m = (MineLayer) robot;
                            foreach (var mine in m.MINES_BY_ID.Values) {
                                battlefieldTurn.AddMine(new ViewerLibrary.Mine(mine.X, mine.Y, false));
                            }
                            break;
                    }
                }

	            foreach (var mine in detonatedMines) { // add detonated mines
                    battlefieldTurn.AddMine(new ViewerLibrary.Mine(mine.X, mine.Y, true));
                }


                turn = battlefieldTurn.ConvertToTurn();
                writer.WriteLine(SERIALIZER.Serialize(turn));

                detonatedMines.Clear();
                heapBullet.Remove(this.turn);
            }
	        
	        turnDataModel?.Add(turn, last);
        }

        private HashSet<BattlefieldRobot> processCommands() {
            battlefieldTurn = new BattlefieldTurn(turn);
            List<Task> tasks = new List<Task>();
            HashSet<BattlefieldRobot> robotsSendCommand = new HashSet<BattlefieldRobot>();
            foreach (KeyValuePair<BattlefieldRobot, ACommand> pair in receivedCommands) {
                BattlefieldRobot robot = pair.Key;
                ACommand command = pair.Value;
                if (command != null) {
                    ACommand answerCommand = commandProcessor.Process(command, new RobotAndBattlefield(robot, this));
                    if (answerCommand != null) {
                        Task t = robot.NETWORK_STREAM.SendCommandAsync(answerCommand);
                        tasks.Add(t);
                    }

                }
                robotsSendCommand.Add(robot);
            }
            Task.WaitAll(tasks.ToArray());
            return robotsSendCommand;
        }


        private void changeBattlefieldState() {
            switch (_battlefieldState) {
                case BattlefieldState.GETTING_EQUIPMENT:
                    _battlefieldState = BattlefieldState.FIGHT;
                    break;
                case BattlefieldState.MERCHANT:
                    _battlefieldState = BattlefieldState.FIGHT;
                    break;
            }
        }


        protected abstract void afterProcessCommand();

        private void moving() {
            foreach (BattlefieldRobot r in robots) {
                if (r.HitPoints > 0) {
                    if (r.Power > r.WantedPower) {
                        r.Power = Math.Max(r.Power - r.Motor.DECELERATION, r.WantedPower);
                    } else {
                        if (r.Power < r.Motor.MAX_INITIAL_POWER) {
                            r.Power = Math.Min(r.Motor.MAX_INITIAL_POWER, r.WantedPower);
                        }
                        r.Power = Math.Min(r.Power + r.Motor.ACCELERATION, r.WantedPower);
                    }
                }

                obstacleManager.MoveChange(r, turn, r.X, r.Y, r.X + RobotUtils.GetSpeedX(r), r.Y + RobotUtils.GetSpeedY(r));
                if (r.X < 0 || r.X >= 1000 || r.Y < 0 || r.Y >= 1000) {
                    r.HitPoints -= (int)Math.Max(1, 5 * r.Power / 100.0);
                    r.Power = 0;
                    r.X = Math.Max(r.X, 0);
                    r.X = Math.Min(r.X, 999);
                    r.Y = Math.Max(r.Y, 0);
                    r.Y = Math.Min(r.Y, 999);
                }
            }
        }

	    public delegate void DamageDealing(Battlefield battlefield);

	    protected void damaging() {
            shooting();
	        detonatingMines();
	        foreach (var dealDamageMethod in damageDealingMethods) {
	            dealDamageMethod(this);
	        }
	    }

	    public void dealDamage(BattlefieldRobot robot, int damage) {
	        robot.HitPoints -= damage;
	        if (robot.HitPoints <= 0) {
	            killedRobots.Add(robot);
	        }
	        robot.HitPoints = Math.Max(0, robot.HitPoints);

        }

        protected void shooting() {
            lock (heapBullet) {
                if (heapBullet.TryGetValue(turn, out List<Bullet> bulletList)) {
                    foreach (Bullet bullet in bulletList) {
                        foreach (BattlefieldRobot r in robots) {
                            if (r.HitPoints > 0) {
                                double distance = EuclideanSpaceUtils.Distance(r.Position, bullet.GetToPosition());
                                Zone zone = Zone.GetZoneByDistance(bullet.TANK.Gun.ZONES, distance);
                                dealDamage(r, zone.EFFECT);

                                if (!bullet.TANK.Equals(r)) {
                                    bullet.TANK.Score += zone.EFFECT;
                                }
                                r.HitPoints = Math.Max(0, r.HitPoints);
                            }
                        }
                    }
                }
            }

            lock (gunLoaded) {
                if (gunLoaded.TryGetValue(turn, out List<Tank> tanksFinishLoading)) {
                    foreach (var tank in tanksFinishLoading) {
                        tank.GunsToLoad--;
                    }
                }
            }
        }

        protected void detonatingMines() {
            lock (detonatedMines) {
                foreach (Mine mine in detonatedMines) {
                    mine.MineLayer.MinesNow--;
                    foreach (BattlefieldRobot r in robots) {
                        if (r.HitPoints > 0) {
                            double distance = EuclideanSpaceUtils.Distance(r.Position, mine.GetPosition());
                            Zone zone = Zone.GetZoneByDistance(mine.MineLayer.MineGun.ZONES, distance);
                            dealDamage(r, zone.EFFECT);
                            if (!mine.MineLayer.Equals(r)) {
                                mine.MineLayer.Score += zone.EFFECT;
                            }
                        }
                    }
                }
            }
        }

        protected abstract void afterMovingAndDamaging();

        private void respawn() {
            if (RESPAWN_ROBOT_AT_TURN.TryGetValue(turn, out List<BattlefieldRobot> respawnedRobots)) {
                foreach (BattlefieldRobot respawnedRobot in respawnedRobots) {
                    respawnedRobot.HitPoints = respawnedRobot.Armor.MAX_HP;
                    Point position = obstacleManager.StartRobotPosition(ARENA_MAX_SIZE, ARENA_MAX_SIZE);
                    respawnedRobot.X = position.X;
                    respawnedRobot.Y = position.Y;
                }
            }
        }

        private void healAllRobots() {
            foreach (var robot in robots) {
                robot.HitPoints = robot.Armor.MAX_HP;
            }
        }

	    private DateTime sendRobotStateCommand(LapState lapState, IEnumerable<BattlefieldRobot> robotsSendCommand) {
            List<BattlefieldRobot> aliveRobots = getAliveRobots();
            int[] aliveRobotsIds = new int[aliveRobots.Count];
            for (int i = 0; i < aliveRobotsIds.Length; i++) {
                aliveRobotsIds[i] = aliveRobots[i].ID;
            }
            DateTime NOW = DateTime.Now;

            List<Task> robotStatesTaskList = new List<Task>();
            foreach (BattlefieldRobot r in robotsSendCommand) {
                robotStatesTaskList.Add(sendRobotStateCommandAsync(lapState, r));
                r.LastRequestAt = NOW;
            }

	        Task.WaitAll(robotStatesTaskList.ToArray());

	        return NOW;
	    }

	    private async Task sendRobotStateCommandAsync(LapState lapState, BattlefieldRobot robotSendCommand) {
	        List<BattlefieldRobot> aliveRobots = getAliveRobots();
	        int[] aliveRobotsIds = new int[aliveRobots.Count];
	        for (int i = 0; i < aliveRobotsIds.Length; i++) {
	            aliveRobotsIds[i] = aliveRobots[i].ID;
	        }
	        
	        EndLapCommand endLapCommand = null;

	        if (lapState != LapState.NONE) {
	            endLapCommand = new EndLapCommand(lapState, robotSendCommand.Gold, robotSendCommand.Score);
	        }
	        RobotStateCommand command = addToRobotStateCommand(new RobotStateCommand((ProtocolDouble)robotSendCommand.X, (ProtocolDouble)robotSendCommand.Y, robotSendCommand.HitPoints, (ProtocolDouble)robotSendCommand.Power, turn, MAX_TURN, aliveRobots.Count, aliveRobotsIds, endLapCommand), robotSendCommand);
	        addObstacleInSight(command, robotSendCommand);
	        await robotSendCommand.NETWORK_STREAM.SendCommandAsync(command);
	    }

        /// <summary>
        /// Robots witch do not send command in time will be disconnected.
        /// </summary>
        /// <param name="NOW">Datetime witch is set in sendRobotStateCommand</param>
	    protected void disconnectTimeoutedAliveRobots(DateTime NOW) {
            List<BattlefieldRobot> aliveRobots = getAliveRobots();
            foreach (BattlefieldRobot r in aliveRobots) {
                if (WAITING_TIME_BETWEEN_TURNS > 0) {
                    if (r.LastRequestAt.Add(MAX_WAITING_TIME).CompareTo(NOW) < 0) {
                        disconnect(r);
                    }
                }
            }
	    }

        /// <summary>
        /// Make single turn simulation
        /// </summary>
	    protected void singleTurnCycle() {
	        lock (receivedCommands) {
	            turn++;
                Console.Write("\rTurn: " + turn);
                HashSet<BattlefieldRobot> robotsSendCommand = processCommands();
                afterProcessCommand();
	            changeBattlefieldState();
	            moving();
	            damaging();
	            detonatingMines();
	            afterMovingAndDamaging();
	            respawn();
	            LapState lapState = newLapState();
	            if (lapState != LapState.NONE && RESPAWN_ALLOWED) {
	                healAllRobots();
	            }
                
	            DateTime now = sendRobotStateCommand(lapState, robotsSendCommand);
	            disconnectTimeoutedAliveRobots(now);
	            handleEndTurn(lapState != LapState.NONE && MAX_LAP == lap);
	            if (RESPAWN_ALLOWED) {
	                foreach (var robot in killedRobots) {
	                    int respawnTurn = turn + RESPAWN_TIMEOUT;
	                    if (!RESPAWN_ROBOT_AT_TURN.TryGetValue(respawnTurn, out List<BattlefieldRobot> respawnedRobots)) {
	                        respawnedRobots = new List<BattlefieldRobot>();
	                        RESPAWN_ROBOT_AT_TURN.Add(respawnTurn, respawnedRobots);
	                    }
	                    respawnedRobots.Add(robot);
	                }
	            }
                killedRobots.Clear();

	            allSendCommand = new TaskCompletionSource<bool>();
                if (lapState != LapState.NONE) {
                    newBattle();
                    _battlefieldState = BattlefieldState.MERCHANT;
                }

                receivedCommands.Clear();
                foreach (BattlefieldRobot r in robotsSendCommand) {
                    listen(r.NETWORK_STREAM);
                }
            }
	    }



        protected void running() {
			while (lap <= MAX_LAP) {
			    if (WAITING_TIME_BETWEEN_TURNS > 0) {
			        Task.WhenAny(Task.Delay(WAITING_TIME_BETWEEN_TURNS), allSendCommand.Task).Wait();
			    } else {
			        allSendCommand.Task.Wait();
			    }
				
                singleTurnCycle();
			}
            writer.Flush();
            writer.Close();
            Thread.Sleep(100);
            _end = true;
		}

        protected async void disconnect(BattlefieldRobot r) {
            r.HitPoints = 0;
            await r.NETWORK_STREAM.SendCommandAsync(new ErrorCommand("Too long without command. You are disconnected"));
            r.NETWORK_STREAM.Close();
        }


	    protected void addObstacleInSight(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        Point[] points = generateSignPoints(r);
            ObstaclesAroundRobot obstaclesInSight = new ObstaclesAroundRobot(obstacleManager.GetObstaclesInPoints(points));
            obstaclesInSight.AddToRobotStateCommand(robotStateCommand);
	    }

	    private static Point[] generateSignPoints(BattlefieldRobot r) {
	        Point[] sight = new Point[11*11];
	        int index = 0;
	        for (int x = -5; x <= 5; x++) {
	            for (int y = -5; y <= 5; y++) {
	                sight[index++] = new Point((int) r.X+x, (int) r.Y+y);
	            }
	        }
	        return sight;
	    }

        protected abstract RobotStateCommand addToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r);

        protected abstract InitAnswerCommand addToInitAnswerCommand(InitAnswerCommand initAnswerCommand);


        protected abstract LapState newLapState();

    }
}
