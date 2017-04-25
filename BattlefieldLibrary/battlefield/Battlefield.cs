using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.equipment;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BattlefieldLibrary.battlefield.robot;
using ObstacleMod;
using ServerLibrary.config;
using ViewerLibrary.serializers;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;
using Point = BaseLibrary.utils.euclidianSpaceStruct.Point;


namespace BattlefieldLibrary.battlefield {
	public abstract partial class Battlefield {
        public const int ARENA_MAX_SIZE = 1000;


        enum BattlefieldState {
	        GET_COMMAND, FIGHT, MERCHANT
	    }

		private const int TIME_FOR_WAIT = 100; //ms. How long wait for receive command in one lap
		private static readonly TimeSpan MAX_WAITING_TIME = new TimeSpan(0, 0, 0, 0, 800); //How long wait for receive command before disconnect

		protected int match = 0;
	    protected LapState LapState;

		public Motor[] Motors { get; private set; }
		public Gun[] Guns { get; private set; }
		public Armor[] Armors { get; private set; }
        public MineGun[] MineGuns { get; private set; }
        public RepairTool[] RepairTools { get; private set; }

        protected List<BattlefieldRobot> robots = new List<BattlefieldRobot>();
		protected List<BattlefieldRobot> pendingRobots = new List<BattlefieldRobot>();
        protected ObtacleManager obtacleManager = new ObtacleManager(new IObtacle[0]);

        private int idForRobot = 0;
		private int activeRobots = 0;


		protected bool _run = false;
		public bool RUN {
			get { return _run; }
			set { setRun(value); }
		}


		private readonly Dictionary<BattlefieldRobot, ACommand> receivedCommands = new Dictionary<BattlefieldRobot, ACommand>();
		private readonly SortedDictionary<int, List<Bullet>> heapBullet = new SortedDictionary<int, List<Bullet>>();
        private readonly IList<Mine> detonatedMines = new List<Mine>();

		private TaskCompletionSource<Boolean> allCommandRecieve = new TaskCompletionSource<Boolean>();

		protected Merchant merchant;
		protected int turn { get; private set; }

	    public readonly int MAX_ROBOTS;
	    public readonly int ROBOTS_IN_TEAM;
	    public readonly int MAX_TURN;
	    public readonly int MATCHES;


        protected BattlefieldTurn battlefieldTurn;
        private ASerializer serializer = new JSONSerializer();
	    private StreamWriter writer;

        private readonly Cache<int, List<BattlefieldRobot>> cache = new Cache<int, List<BattlefieldRobot>>(true);
        BattlefieldState _battlefieldState = BattlefieldState.GET_COMMAND;

        MerchantCommandVisitor merchantCommandVisitor; 
        GetCommandVisitor getCommandVisitor;
        TankFightCommandVisitor tankFightCommandVisitor;
        MinerFightCommandVisitor minerFightCommandVisitor;
	    RepairmanFightCommandVisitor repairmanFightCommandVisitor;

	    protected Battlefield(int maxRobots, int maxTurn, int robotsInTeam, String equipmentConfigFile) {
            this.ROBOTS_IN_TEAM = robotsInTeam;
            this.MAX_ROBOTS = maxRobots;
            this.MATCHES = 3;
            MAX_TURN = maxTurn;
            ServerConfig.SetEquipmentFromFile(equipmentConfigFile);
	        battlefieldSetting(ServerConfig.MOTORS, ServerConfig.GUNS, ServerConfig.ARMORS, ServerConfig.REPAIR_TOOLS, ServerConfig.MINE_GUNS);
	    }

        protected Battlefield(int maxRobots, int maxTurn, int robotsInTeam) {
            this.ROBOTS_IN_TEAM = robotsInTeam;
            this.MAX_ROBOTS = maxRobots;
            this.MATCHES = 3;
            MAX_TURN = maxTurn;
            ServerConfig.SetDefaultEquipment();
            battlefieldSetting(ServerConfig.MOTORS, ServerConfig.GUNS, ServerConfig.ARMORS, ServerConfig.REPAIR_TOOLS, ServerConfig.MINE_GUNS);
        }

        private void battlefieldSetting(Motor[] motors, Gun[] guns, Armor[] armors, RepairTool[] repairTools, MineGun[] mineGuns) {
            this.Motors = motors;
			this.Guns = guns;
			this.Armors = armors;
            this.MineGuns = mineGuns;
            this.RepairTools = repairTools;

            
			merchant = new Merchant(motors, armors, guns, repairTools, mineGuns);
            writer = new StreamWriter(File.OpenWrite("arena_match.txt")) {AutoFlush = true};
            writer.WriteLine(serializer.Config());

            merchantCommandVisitor = new MerchantCommandVisitor(this);
            getCommandVisitor = new GetCommandVisitor(this);
            tankFightCommandVisitor = new TankFightCommandVisitor(this);
            minerFightCommandVisitor = new MinerFightCommandVisitor(this);
            repairmanFightCommandVisitor = new RepairmanFightCommandVisitor(this);
        }

	    protected ArenaCommandVisitor GetActualArenaCommandVisitor(BattlefieldRobot r) {
	        switch (_battlefieldState) {
	            case BattlefieldState.GET_COMMAND:
	                return getCommandVisitor;
	            case BattlefieldState.FIGHT:
	                switch (r.ROBOT_TYPE) {
	                    case RobotType.MINER:
	                        return minerFightCommandVisitor;
	                    case RobotType.TANK:
	                        return tankFightCommandVisitor;
	                    case RobotType.REPAIRMAN:
	                        return repairmanFightCommandVisitor;
	                    default:
	                        throw new NotSupportedException("Unsupported robot type");
	                }
                case BattlefieldState.MERCHANT:
	                return merchantCommandVisitor;
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

		protected async void listen(BattlefieldRobot r) {
			await Task.Yield();
			try {
				ACommand command = await r.SuperNetworkStream.RecieveCommandAsync();
				
				if (command is GetArmorsCommand || command is GetMotorsCommand || command is GetGunsCommand || command is GetMineGunCommand || command is GetRepairToolCommand) {
					command.accept(GetActualArenaCommandVisitor(r), r);
				    listen(r);
				}  else {
			        if (command is InitCommand) {
			            lock (pendingRobots) {
			                activeRobots++;
			                pendingRobots.Remove(r);
			            }

			            lock (robots) {
			                robots.Add(r);
			                if (robots.Count == MAX_ROBOTS) {
			                    setRun(true);
			                }
			            }
			        }
                    lock (receivedCommands) {
                        receivedCommands.Add(r, command);
                        lock (robots) {
                            if (_run && receivedCommands.Count == robots.Count) {
                                allCommandRecieve.SetResult(true);
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
			}
		}

		public bool AddRobot(SuperNetworkStream n) {
			bool ret = false;
		    lock (pendingRobots) {
		        if (!RUN && (MAX_ROBOTS <= 0 || activeRobots + pendingRobots.Count < MAX_ROBOTS)) {
		            BattlefieldRobot r = new DefaultRobot(idForRobot++, n);
		            pendingRobots.Add(r);
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
				running();
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
                    tank.BulletsNow = 0;
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
			turn = 0;
		    match++;
			foreach (BattlefieldRobot r in robots) {
				r.Power = 0;
				r.WantedPower = 0;
			    Point position = obtacleManager.StartRobotPosition(ARENA_MAX_SIZE, ARENA_MAX_SIZE);

                r.X = position.X;
				r.Y = position.Y;

                Tank tank = r as Tank;
                if (tank != null) {
                    tank.BulletsNow = 0;
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
		}

		protected virtual void firstBattle() {
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

		protected double srandardizeDegree(double degree) {
			double resul = degree;
			if (degree < 0) {
				resul = 360 - (-degree % 360);
			}
			return resul % 360;
		}

		protected double computeAngle(double x1, double y1, double x2, double y2) {
			return srandardizeDegree(AngleUtils.Angle(x1, y1, x2, y2));
		}

		protected void moving() {
			foreach (BattlefieldRobot r in robots) {
				if (r.HitPoints > 0) {
					if (r.Power > r.WantedPower) {
						r.Power = Math.Max(r.Power - r.Motor.SPEED_DOWN, r.WantedPower);
					} else {
						r.Power = Math.Min(r.Motor.SPEED_UP_TO, r.WantedPower);
						r.Power = Math.Min(r.Power + r.Motor.SPEED_UP, r.WantedPower);
					}
				}
                
				obtacleManager.MoveChange(r, turn, r.X, r.Y, r.X + RobotUtils.getSpeedX(r), r.Y + RobotUtils.getSpeedY(r));
				if (r.X < 0 || r.X >= 1000 || r.Y < 0 || r.Y >= 1000) {
					r.HitPoints -= (int) Math.Max(1, 5 * r.Power/100.0);
					r.Power = 0;
					r.X = Math.Max(r.X, 0);
					r.X = Math.Min(r.X, 999);
					r.Y = Math.Max(r.Y, 0);
					r.Y = Math.Min(r.Y, 999);
				}
			}
		}

		protected void shotting() {
			List<Bullet> bulletList;
			if (heapBullet.TryGetValue(turn, out bulletList)) {
				foreach (Bullet bullet in bulletList) {
					bullet.TANK.BulletsNow--;
					foreach (BattlefieldRobot r in robots) {
						if (r.HitPoints > 0) {
							double distance = Math.Sqrt(Math.Pow(r.X - bullet.TO_X, 2) + Math.Pow(r.Y - bullet.TO_Y, 2));
							foreach (Zone zone in bullet.TANK.Gun.ZONES) {
								if (zone.DISTANCE > distance) {
									r.HitPoints -= zone.EFFECT;
									if (bullet.TANK != r) {
										bullet.TANK.Score += zone.EFFECT;
									}
									break;
								}
							}
						}
					}
				}
			}
		}

	    protected void detonateMines() {
            foreach (Mine mine in detonatedMines) {
                mine.MINER.MinesNow--;
                foreach (BattlefieldRobot r in robots) {
                    if (r.HitPoints > 0) {
                        double distance = Math.Sqrt(Math.Pow(r.X - mine.X, 2) + Math.Pow(r.Y - mine.Y, 2));
                        foreach (Zone zone in mine.MINER.MineGun.ZONES) {
                            if (zone.DISTANCE > distance) {
                                r.HitPoints -= zone.EFFECT;
                                if (mine.MINER != r) {
                                    mine.MINER.Score += zone.EFFECT;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        protected virtual void handleEndTurn() {
	        lock (writer) {
	            foreach (BattlefieldRobot r in robots) {
                    battlefieldTurn.AddRobot(new ViewerLibrary.Robot(r.TEAM_ID, r.OldScore, r.Gold, r.HitPoints, r.X, r.Y, r.AngleDrive, r.NAME));
                }

                foreach (var pairValueKeybullets in heapBullet) {
                    foreach (var bullet in pairValueKeybullets.Value) {
                        int difference = turn - bullet.FROM_LAP;
                        double speed_x = (double)(bullet.TO_X - bullet.FROM_X) / (bullet.TO_LAP - bullet.FROM_LAP);
                        double speed_y = (double)(bullet.TO_Y - bullet.FROM_Y) / (bullet.TO_LAP - bullet.FROM_LAP);
                        double x = bullet.FROM_X + difference * speed_x;
                        double y = bullet.FROM_Y + difference * speed_y;
                        battlefieldTurn.AddBullet(new ViewerLibrary.Bullet(x, y, pairValueKeybullets.Key == turn));
                    }
                }

	            writer.WriteLine(serializer.Serialize(battlefieldTurn.ConvertToTurn()));

	            heapBullet.Remove(turn);
            }
        }


        protected async void running() {
			while (match <= MATCHES) {
				await Task.Yield();
				await Task.WhenAny(Task.Delay(TIME_FOR_WAIT), allCommandRecieve.Task);
				lock (receivedCommands) {

                    turn++;
                    LapState = newLap();
                    battlefieldTurn = new BattlefieldTurn(turn);

                    IEnumerable<BattlefieldRobot> robotsWithCommand = (IEnumerable<BattlefieldRobot>) receivedCommands.Keys;
					foreach (KeyValuePair<BattlefieldRobot, ACommand> pair in receivedCommands) {
						pair.Value.accept(GetActualArenaCommandVisitor(pair.Key), pair.Key);           
                    }

                    switch (_battlefieldState) {
                        case BattlefieldState.GET_COMMAND:
                            _battlefieldState = BattlefieldState.FIGHT;
                            robotsWithCommand = this.robots;
                            break;
                        case BattlefieldState.MERCHANT:
                            _battlefieldState = BattlefieldState.FIGHT;
                            break;
                    }

                    var aliveRobots = getAliveRobots();
					int[] aliveRobotsIds = new int[aliveRobots.Count];
					for (int i = 0; i < aliveRobotsIds.Length; i++) {
						aliveRobotsIds[i] = aliveRobots[i].ID;
					}

					DateTime NOW = new DateTime();
					foreach (BattlefieldRobot r in robotsWithCommand) {
					    EndLapCommand endLapCommand = null;

                        if (LapState != LapState.NONE) {
                            endLapCommand = new EndLapCommand(LapState, r.Gold, r.Score);
                        }
					    RobotStateCommand command = AddToRobotStateCommand(new RobotStateCommand((ProtocolDouble) r.X, (ProtocolDouble) r.Y, r.HitPoints, (ProtocolDouble) r.Power, turn, MAX_TURN, aliveRobots.Count, aliveRobotsIds, endLapCommand), r);
						r.SuperNetworkStream.SendCommandAsyncDontWait(command);
						r.LastRequestAt = NOW;
					}

					foreach (BattlefieldRobot r in robots) {
						if (r.HitPoints > 0) {
							if (r.LastRequestAt.Add(MAX_WAITING_TIME).CompareTo(NOW) < 0) {
								disconnect(r);
							}
						}
					}

                    handleEndTurn();

                    allCommandRecieve = new TaskCompletionSource<Boolean>();
                    if (LapState != LapState.NONE) {
                        newBattle();
                        _battlefieldState = BattlefieldState.MERCHANT;
                    }

                    foreach (BattlefieldRobot r in robotsWithCommand) {
						listen(r);
					}

                    
                    receivedCommands.Clear();
                }
			}
            if (match > MATCHES) {
                writer.Flush();
                writer.Close();
                _end = true;
            }
		}

        protected LapState newLap() {
            turn++;
            moving();
            shotting();
            detonateMines();
            foreach (BattlefieldRobot r in robots) {
                r.HitPoints = Math.Max(0, r.HitPoints);
            }
            return newLapState();
        }


        protected async void disconnect(BattlefieldRobot r) {
            robots.Remove(r);
            r.HitPoints = 0;
            await r.SuperNetworkStream.SendCommandAsync(new ErrorCommand("To long without command. You are disconnected"));
            r.SuperNetworkStream.Close();
        }

        protected abstract RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r);

        protected abstract InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand);

        protected abstract LapState newLapState();

    }
}
