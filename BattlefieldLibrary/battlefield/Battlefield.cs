using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.equipment;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;
using BaseLibrary.protocol;
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
        public const int ARENA_MAX_SIZE = 1000;


        enum BattlefieldState {
	        GET_COMMAND, FIGHT, MERCHANT
	    }

		protected int lap = 0;
	    //protected LapState LapState;

		public Motor[] Motors { get; private set; }
		public Gun[] Guns { get; private set; }
		public Armor[] Armors { get; private set; }
        public MineGun[] MineGuns { get; private set; }
        public RepairTool[] RepairTools { get; private set; }

        protected List<BattlefieldRobot> robots = new List<BattlefieldRobot>();
	    protected Dictionary<int, BattlefieldRobot> robotsById = new Dictionary<int, BattlefieldRobot>();
        protected Dictionary<string, int> robotTeamIdByTeamName = new Dictionary<string, int>();
        protected Dictionary<int, List<BattlefieldRobot>> robotsByTeamId = new Dictionary<int, List<BattlefieldRobot>>();
        protected List<BattlefieldRobot> pendingRobots = new List<BattlefieldRobot>();
        protected ObstacleManager obtacleManager;

        private int idForRobot = 1;
		private int activeRobots = 0;

	    private readonly bool RESPAWN_ALLOWED;
	    private readonly int WAITING_TIME_BETWEEN_TURNS;
	    private readonly TimeSpan MAX_WAITING_TIME;

        protected bool _run = false;
		public bool RUN {
			get => _run;
		    set => setRun(value);
		}

        /// <summary>
        /// This mutex is set when arena start.
        /// </summary>
        public readonly ManualResetEvent RunEvent = new ManualResetEvent(false);

        /// <summary>
        /// This thread is only for waint until simulation will end. It start when all robots are connected.
        /// </summary>
        public readonly Thread RunThread;

        
		private readonly Dictionary<BattlefieldRobot, ACommand> receivedCommands = new Dictionary<BattlefieldRobot, ACommand>();
		private readonly IDictionary<int, List<Bullet>> heapBullet = new SortedDictionary<int, List<Bullet>>();
        private readonly IDictionary<int, List<Tank>> gunLoaded = new SortedDictionary<int, List<Tank>>();
        private readonly IList<Mine> detonatedMines = new List<Mine>();

		private TaskCompletionSource<bool> allSendCommand = new TaskCompletionSource<bool>();

		protected Merchant Merchant;
		protected int Turn { get; private set; }

	    public readonly int TEAMS;
	    public readonly int ROBOTS_IN_TEAM;
	    public readonly int MAX_TURN;
	    public readonly int MAX_LAP;
	    public readonly int RESPAWN_TIMEOUT = 0;

        protected BattlefieldTurn battlefieldTurn;
        private readonly JSONSerializer SERIALIZER = new JSONSerializer();
	    private StreamWriter writer;

        private readonly Cache<int, List<BattlefieldRobot>> cache = new Cache<int, List<BattlefieldRobot>>(true);
        BattlefieldState _battlefieldState = BattlefieldState.GET_COMMAND;

        private MerchantVisitor merchantVisitor;
	    private GetVisitor getVisitor;
	    private TankFightVisitor tankFightVisitor;
	    private MinerFightVisitor minerFightVisitor;
	    private RepairmanFightVisitor repairmanFightVisitor;

        private readonly Dictionary<int, List<BattlefieldRobot>> RESPAWN_ROBOT_AT_TURN = new Dictionary<int, List<BattlefieldRobot>>();

	    private readonly SerialTurnDataModel TURN_DATA_MODEL;

       protected Battlefield(BattlefieldConfig battlefieldConfig) {
            ROBOTS_IN_TEAM = battlefieldConfig.ROBOTS_IN_TEAM;
            TEAMS = battlefieldConfig.TEAMS;
            MAX_LAP = battlefieldConfig.MAX_LAP;
            MAX_TURN = battlefieldConfig.MAX_TURN;
            RESPAWN_TIMEOUT = battlefieldConfig.RESPAWN_TIMEOUT;
            RESPAWN_ALLOWED = battlefieldConfig.RESPAWN_ALLOWED;
            WAITING_TIME_BETWEEN_TURNS = battlefieldConfig.WAITING_TIME_BETWEEN_TURNS;
            MAX_WAITING_TIME = new TimeSpan(0, 0, 0, 0, 8 * WAITING_TIME_BETWEEN_TURNS); //How long wait for receive command before disconnect

            if (battlefieldConfig.GUI) {
                TURN_DATA_MODEL = new SerialTurnDataModel();
                
                Thread t = new Thread( () => {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new BattlefieldViewer(TURN_DATA_MODEL));
                });
                t.Start();
            }

            if (battlefieldConfig.EQUIPMENT_CONFIG_FILE != null) {
	            ServerConfig.SetEquipmentFromFile(battlefieldConfig.EQUIPMENT_CONFIG_FILE);
	        } else {
	            ServerConfig.SetDefaultEquipment();
	        }
            IEnumerable<IObstacle> obstacles = (battlefieldConfig.OBTACLE_CONFIG_FILE != null) ? (IEnumerable<IObstacle>) ObstacleManager.LoadObtaclesFromFile(battlefieldConfig.OBTACLE_CONFIG_FILE) : new IObstacle[0];
	        obtacleManager = new ObstacleManager(obstacles);
	       
	        battlefieldSetting(ServerConfig.MOTORS, ServerConfig.GUNS, ServerConfig.ARMORS, ServerConfig.REPAIR_TOOLS, ServerConfig.MINE_GUNS, battlefieldConfig.MATCH_SAVE_FILE);

            RunThread = new Thread(new ThreadStart(this.running));
        }

        private void battlefieldSetting(Motor[] motors, Gun[] guns, Armor[] armors, RepairTool[] repairTools, MineGun[] mineGuns, string filename) {
            this.Motors = motors;
			this.Guns = guns;
			this.Armors = armors;
            this.MineGuns = mineGuns;
            this.RepairTools = repairTools;

            
			Merchant = new Merchant(motors, armors, guns, repairTools, mineGuns);

            if (File.Exists(filename)) {
                File.Delete(filename);
            }

            writer = new StreamWriter(File.OpenWrite(filename)) {AutoFlush = true};

            merchantVisitor = new MerchantVisitor(this);
            getVisitor = new GetVisitor(this);
            tankFightVisitor = new TankFightVisitor(this);
            minerFightVisitor = new MinerFightVisitor(this);
            repairmanFightVisitor = new RepairmanFightVisitor(this);
        }

	    protected ArenaVisitor GetActualArenaCommandVisitor(BattlefieldRobot r) {
	        switch (_battlefieldState) {
	            case BattlefieldState.GET_COMMAND:
	                return getVisitor;
	            case BattlefieldState.FIGHT:
	                switch (r.ROBOT_TYPE) {
	                    case RobotType.MINER:
	                        return minerFightVisitor;
	                    case RobotType.TANK:
	                        return tankFightVisitor;
	                    case RobotType.REPAIRMAN:
	                        return repairmanFightVisitor;
	                    default:
	                        throw new NotSupportedException("Unsupported robot type");
	                }
                case BattlefieldState.MERCHANT:
	                return merchantVisitor;
                default:
                    throw new NotSupportedException("Unsupported arenaState");
            }
	    }

	    private bool _end = false;
        public bool End() {
	        return _end;
	    }

		public void Start() {
			setRun(true);
		}

        protected readonly HashSet<BattlefieldRobot> robotsSendCommand = new HashSet<BattlefieldRobot>();
        protected readonly HashSet<BattlefieldRobot> robotsWaitingForReborn = new HashSet<BattlefieldRobot>();

        protected async void listen(BattlefieldRobot r) {
			await Task.Yield();
            try {
                ACommand command = await r.SuperNetworkStream.RecieveCommandAsync();

                if (command is AEquipmentCommand) {
                    r.SuperNetworkStream.SendCommand(command.accept(GetActualArenaCommandVisitor(r), r));
                    listen(r);
                } else {
                    if (command is InitCommand) {
                        lock (pendingRobots) {
                            activeRobots++;
                            pendingRobots.Remove(r);
                        }

                        lock (robots) {
                            robots.Add(r);

                            if (robots.Count == TEAMS*ROBOTS_IN_TEAM) {
                                setRun(true);
                            }
                        }
                    }
                    lock (receivedCommands) {
                        receivedCommands.Add(r, command);
                        robotsSendCommand.Remove(r);
                        lock (robots) {
                            if (_run && 0 == robotsSendCommand.Count) {
                                allSendCommand.SetResult(true);
                            }
                        }
                    }
                }

            } catch (IOException) {
                // client was disconnected
                lock (robots) {
                    robots.Remove(r);
                }
            } catch (ObjectDisposedException) {
                // server disconnect client
                lock (robots) {
                    robots.Remove(r);
                }
            } catch (ArgumentException e) {
                lock (robots) {
                    robots.Remove(r);
                    Console.WriteLine(e.Message);
                }
            }
		}

		public bool AddRobot(SuperNetworkStream n) {
			bool ret = false;
		    lock (pendingRobots) {
		        if (!RUN && (TEAMS * ROBOTS_IN_TEAM <= 0 || activeRobots + pendingRobots.Count < TEAMS * ROBOTS_IN_TEAM)) {
		            BattlefieldRobot r = new DefaultRobot(idForRobot++, n);
		            pendingRobots.Add(r);
		            robotsSendCommand.Add(r);
                    listen(r);
		            ret = true;
		        }
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
				firstBattle();
				RunThread.Start();
			    RunEvent.Set();
			}
		}

		public bool isRun() {
			return RUN;
		}

		protected virtual void initEquip() {
			foreach (BattlefieldRobot r in robots) {
				r.Motor = Motors[0];
                r.Armor = Armors[0];

                Tank tank = r as Tank;
                if (tank != null) {
                    tank.GunsToLoad = 0;
                }

                Miner miner = r as Miner;
                if (miner != null) {
                    miner.MinesNow = 0;
                    miner.MINES_BY_ID.Clear();
                }

                Repairman repairman = r as Repairman;
                if (repairman != null) {
                    repairman.RepairToolUsed = 0;
                }
            }
		}

        protected void newBattle() {
			Turn = 0;
		    lap++;
			foreach (BattlefieldRobot r in robots) {
				r.Power = 0;
				r.WantedPower = 0;
			    Point position = obtacleManager.StartRobotPosition(ARENA_MAX_SIZE, ARENA_MAX_SIZE);

                r.X = position.X;
				r.Y = position.Y;

                Tank tank = r as Tank;
                if (tank != null) {
                    tank.GunsToLoad = 0;
                }

                Miner miner = r as Miner;
                if (miner != null) {
                    miner.MinesNow = 0;
                    miner.MINES_BY_ID.Clear();
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
            initEquip();
            newBattle();
			foreach (BattlefieldRobot r in robots) {
				r.HitPoints = r.Armor.MAX_HP;
			}
			foreach (BattlefieldRobot r in pendingRobots) {
				r.SuperNetworkStream.Close();
			}
		}

		protected List<BattlefieldRobot> getAliveRobots() {
			if (cache.IsCached(Turn)) {
				return cache.GetCached();
			}
			var aliveRobots = new List<BattlefieldRobot>();
			foreach (BattlefieldRobot r in robots) {
				if (r.HitPoints > 0) {
					aliveRobots.Add(r);
				}
			}
			cache.Cached(Turn, aliveRobots);
			return aliveRobots;
		}

	    protected virtual void handleEndTurn(bool last) {
	        Turn turn;
            lock (writer) {
	            foreach (BattlefieldRobot r in robots) {
	                if (r.HitPoints > 0) {
	                    battlefieldTurn.AddRobot(new ViewerLibrary.Robot(r.TEAM_ID, r.Score, r.Gold, r.HitPoints, r.X, r.Y,
	                                                                     r.AngleDrive, r.NAME));
	                }
	            }

                foreach (var pairValueKeyBullets in heapBullet) { // add bullets
                    foreach (var bullet in pairValueKeyBullets.Value) {
                        int difference = Turn - bullet.FROM_LAP;
                        double speedX = (bullet.TO_X - bullet.FROM_X) / (bullet.TO_LAP - bullet.FROM_LAP);
                        double speedY = (bullet.TO_Y - bullet.FROM_Y) / (bullet.TO_LAP - bullet.FROM_LAP);
                        double x = bullet.FROM_X + difference * speedX;
                        double y = bullet.FROM_Y + difference * speedY;
                        battlefieldTurn.AddBullet(new ViewerLibrary.Bullet(x, y, pairValueKeyBullets.Key == Turn));
                    }
                }

                foreach (var robot in robots) { // add not detonated mines
                    switch (robot.ROBOT_TYPE) {
                        case RobotType.MINER:
                            Miner m = (Miner) robot;
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
                heapBullet.Remove(Turn);
            }
	        
	        TURN_DATA_MODEL?.Add(turn, last);
        }

        private void processCommands() {
            battlefieldTurn = new BattlefieldTurn(Turn);
            robotsSendCommand.Clear();
            List<Task> tasks = new List<Task>();

            foreach (KeyValuePair<BattlefieldRobot, ACommand> pair in receivedCommands) {
                BattlefieldRobot robot = pair.Key;
                ACommand command = pair.Value;
                ACommand answerCommand = command.accept(GetActualArenaCommandVisitor(robot), robot);
                if (answerCommand != null) {
                    Task t = robot.SuperNetworkStream.SendCommandAsync(answerCommand);
                    tasks.Add(t);
                }
                if (command is InitCommand) {
                    switch (pair.Key.ROBOT_TYPE) {
                        case RobotType.MINER:
                            robot = new Miner(robot);
                            ((Miner) robot).MineGun = MineGuns[0];
                            break;
                        case RobotType.REPAIRMAN:
                            robot = new Repairman(robot);
                            ((Repairman)robot).RepairTool = RepairTools[0];
                            break;
                        case RobotType.TANK:
                            robot = new Tank(robot);
                            ((Tank)robot).Gun = Guns[0];
                            break;
                    }
                }
                robotsSendCommand.Add(robot);
            }
            Task.WaitAll(tasks.ToArray());
        }


        private void changeBattlefieldState() {
            switch (_battlefieldState) {
                case BattlefieldState.GET_COMMAND:
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
                        r.Power = Math.Max(r.Power, Math.Min(r.Motor.MAX_INITIAL_POWER, r.WantedPower));
                        r.Power = Math.Min(r.Power + r.Motor.ACCELERATION, r.WantedPower);
                    }
                }

                obtacleManager.MoveChange(r, Turn, r.X, r.Y, r.X + RobotUtils.GetSpeedX(r), r.Y + RobotUtils.GetSpeedY(r));
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

        protected void shooting() {
            if (heapBullet.TryGetValue(Turn, out List<Bullet> bulletList)) {
                foreach (Bullet bullet in bulletList) {
                    foreach (BattlefieldRobot r in robots) {
                        if (r.HitPoints > 0) {
                            double distance = EuclideanSpaceUtils.Distance(r.GetPosition(), bullet.GetToPosition());
                            Zone zone = Zone.GetZoneByDistance(bullet.TANK.Gun.ZONES, distance);
                            r.HitPoints -= zone.EFFECT;
                            if (bullet.TANK != r) {
                                bullet.TANK.Score += zone.EFFECT;
                            }
                            r.HitPoints = Math.Max(0, r.HitPoints);
                        }
                    }
                }
            }

            if (gunLoaded.TryGetValue(Turn, out List<Tank> tanksFinishLoading)) {
                foreach (var tank in tanksFinishLoading) {
                    tank.GunsToLoad--;
                }
            }
        }

        protected void detonatingMines() {
            foreach (Mine mine in detonatedMines) {
                mine.MINER.MinesNow--;
                foreach (BattlefieldRobot r in robots) {
                    if (r.HitPoints > 0) {
                        double distance = EuclideanSpaceUtils.Distance(r.GetPosition(), mine.GetPosition());
                        Zone zone = Zone.GetZoneByDistance(mine.MINER.MineGun.ZONES, distance);
                        r.HitPoints -= zone.EFFECT;
                        if (mine.MINER != r) {
                            mine.MINER.Score += zone.EFFECT;
                        }
                        r.HitPoints = Math.Max(0, r.HitPoints);
                    }
                }
            }
        }

        protected abstract void afterMovingAndDamaging();

        private void respawn() {
            if (RESPAWN_ROBOT_AT_TURN.TryGetValue(Turn, out List<BattlefieldRobot> respawnedRobots)) {
                foreach (BattlefieldRobot respawnedRobot in respawnedRobots) {
                    respawnedRobot.HitPoints = respawnedRobot.Armor.MAX_HP;
                    Point position = obtacleManager.StartRobotPosition(ARENA_MAX_SIZE, ARENA_MAX_SIZE);
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

	    private DateTime sendRobotStateCommand(LapState lapState) {
            List<BattlefieldRobot> aliveRobots = getAliveRobots();
            int[] aliveRobotsIds = new int[aliveRobots.Count];
            for (int i = 0; i < aliveRobotsIds.Length; i++) {
                aliveRobotsIds[i] = aliveRobots[i].ID;
            }
            DateTime NOW = DateTime.Now;

            foreach (var waitingRobot in robotsWaitingForReborn) {
                if (waitingRobot.HitPoints > 0) {
                    robotsSendCommand.Add(waitingRobot);
                    robotsWaitingForReborn.Remove(waitingRobot);
                }
            }

            if (lapState != LapState.NONE) {
                foreach (var waitingRobot in robotsWaitingForReborn) {
                    robotsSendCommand.Add(waitingRobot);
                }
            }

            List<Task> robotStatesTaskList = new List<Task>();
            foreach (BattlefieldRobot r in robotsSendCommand) {
                EndLapCommand endLapCommand = null;

                if (lapState != LapState.NONE) {
                    endLapCommand = new EndLapCommand(lapState, r.Gold, r.Score);
                }
                RobotStateCommand command = AddToRobotStateCommand(new RobotStateCommand((ProtocolDouble) r.X, (ProtocolDouble) r.Y, r.HitPoints, (ProtocolDouble) r.Power, Turn, MAX_TURN, aliveRobots.Count, aliveRobotsIds, endLapCommand), r);
                AddObtacleInSight(command, r);
                robotStatesTaskList.Add(r.SuperNetworkStream.SendCommandAsync(command));
                r.LastRequestAt = NOW;
            }

	        Task.WaitAll(robotStatesTaskList.ToArray());

	        return NOW;
	    }

        /// <summary>
        ///
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

	    protected void singleTurnCycle() {
	        lock (receivedCommands) {
	            Turn++;
                Console.WriteLine("Turn: " + Turn);
                processCommands();
                afterProcessCommand();
	            changeBattlefieldState();
	            moving();
	            shooting();
	            detonatingMines();
	            afterMovingAndDamaging();
	            respawn();
	            LapState lapState = NewLapState();
	            if (lapState != LapState.NONE && RESPAWN_ALLOWED) {
	                healAllRobots();
	            }
                
	            DateTime now = sendRobotStateCommand(lapState);
	            disconnectTimeoutedAliveRobots(now);
	            handleEndTurn(lapState != LapState.NONE && MAX_LAP == lap);
	            foreach (var robot in robots) {
                    if (robot.HitPoints <= 0) {
                        int respawnTurn = Turn + RESPAWN_TIMEOUT;
                        if (!RESPAWN_ROBOT_AT_TURN.TryGetValue(respawnTurn, out List<BattlefieldRobot> respawnedRobots)) {
                            respawnedRobots = new List<BattlefieldRobot>();
                            RESPAWN_ROBOT_AT_TURN.Add(respawnTurn, respawnedRobots);
                        }
                        respawnedRobots.Add(robot);
                    }
                }

                allSendCommand = new TaskCompletionSource<bool>();
                if (lapState != LapState.NONE) {
                    newBattle();
                    _battlefieldState = BattlefieldState.MERCHANT;
                }

                receivedCommands.Clear();
                foreach (BattlefieldRobot r in robotsSendCommand) {
                    listen(r);
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
            await r.SuperNetworkStream.SendCommandAsync(new ErrorCommand("Too long without command. You are disconnected"));
            r.SuperNetworkStream.Close();
        }


	    protected void AddObtacleInSight(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        Point[] points = generateSignPoints(r);
            ObstaclesAroundRobot obtaclesInSight = new ObstaclesAroundRobot(obtacleManager.GetObtaclesInPoints(points));
            obtaclesInSight.AddToRobotStateCommand(robotStateCommand);
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

        protected abstract RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r);

        protected abstract InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand);


        protected abstract LapState NewLapState();

    }
}
