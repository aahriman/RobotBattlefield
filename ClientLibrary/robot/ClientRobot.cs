using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.communication;
using BaseLibrary.communication.command;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.config;
using BaseLibrary.equipment;
using BaseLibrary.utils;
using ClientLibrary.config;

namespace ClientLibrary.robot {

    /// <summary>
    /// Abstract class for robot's classes at client side. Implement all common methods.
    /// </summary>
    public abstract class ClientRobot : Robot {

        /// <summary>
        /// Team name which is globally unique.
        /// </summary>
        public static readonly String TEAM_NAME = Guid.NewGuid().ToString();

        private static readonly List<ClientRobot> ROBOT_COLLECTION = new List<ClientRobot>();
        private static readonly IDictionary<ClientRobot, Task> ROBOTS_TASK_COMMANDS = new Dictionary<ClientRobot, Task>();

        private static String ip = null;
        private static int port;

        private static readonly String ERROR_FILE_NAME = "error.txt";

        static ClientRobot() {
            ModUtils.LoadMods();

            if (!System.Diagnostics.Debugger.IsAttached) { // add handler for non debugging

                uint numberOfTry = 0;
                while (true) {
                    numberOfTry++;
                    try {
                        ERROR_FILE_NAME = errorName(numberOfTry);
                        Console.SetError(new IndentedTextWriter(File.AppendText(ERROR_FILE_NAME)));
                        break;
                    } catch {
                        // cannot open file, try next one
                        if (numberOfTry > 1000) {
                            Console.WriteLine("Cannot open error file. Application will be closed.");
                            Thread.Sleep(1000);
                            Environment.Exit(2);
                        }
                    }
                }

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
                Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            }
        }

        private static String errorName(uint numberOfTry) {
            return $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}-error-{numberOfTry}.txt";
        }


        /// <summary>
        /// Motors available to buy by its id.
        /// </summary>
        public static readonly Dictionary<int, Motor> MOTORS_BY_ID = new Dictionary<int, Motor>();

        /// <summary>
        /// Motors available to buy.
        /// </summary>
        public static Dictionary<int, Motor>.ValueCollection MOTORS => MOTORS_BY_ID.Values;


        /// <summary>
        /// Armors available to buy by its id.
        /// </summary>
        public static readonly Dictionary<int, Armor> ARMORS_BY_ID = new Dictionary<int, Armor>();

        /// <summary>
        /// Armors available to buy.
        /// </summary>
        public static Dictionary<int, Armor>.ValueCollection ARMORS => ARMORS_BY_ID.Values;

        /// <summary>
        /// Guns available to buy by its id.
        /// </summary>
        public static readonly Dictionary<int, Gun> GUNS_BY_ID = new Dictionary<int, Gun>();

        /// <summary>
        /// Guns available to buy.
        /// </summary>
        public static Dictionary<int, Gun>.ValueCollection GUNS => GUNS_BY_ID.Values;

        /// <summary>
        /// Repair tools available to buy by its id.
        /// </summary>
        public static readonly Dictionary<int, RepairTool> REPAIR_TOOLS_BY_ID = new Dictionary<int, RepairTool>();
        /// <summary>
        /// Repair tools available to buy.
        /// </summary>
        public static Dictionary<int, RepairTool>.ValueCollection REPAIR_TOOLS => REPAIR_TOOLS_BY_ID.Values;

        /// <summary>
        /// Mine guns available to buy by its id.
        /// </summary>
        public static readonly Dictionary<int, MineGun> MINE_GUNS_BY_ID = new Dictionary<int, MineGun>();
        /// <summary>
        /// Mine guns available to buy.
        /// </summary>
        public static Dictionary<int, MineGun>.ValueCollection MINE_GUNS => MINE_GUNS_BY_ID.Values;


        /// <summary>
        /// Do connection to server.
        /// </summary>
        /// <param name="args">
        ///   <list type="bullet">
        ///     <item>
        ///       <description><code>args[0]</code> is IP adress of server.</description>
        ///     </item>
        ///     <item>
        ///       <description><code>args[1]</code> is port.</description>
        ///     </item>
        ///     <item>
        ///       <description>If <code>args.length &lt; 2</code> then default port is choose, if <code>args.length &lt; 1</code> then local adress is use as ip.</description>
        ///     </item>
        ///   </list>
        /// </param>
        /// <returns>Game type</returns>
        public static GameTypeCommand Connect(String[] args) {
            GameTypeCommand gameTypeCommand = null;
            lock (ROBOT_COLLECTION) {
                ip = ConnectionUtil.LOCAL_ADDRESS;
                port = GameProperties.DEFAULT_PORT;
                if (args.Length >= 1) {
                    ip = args[0];
                }

                if (args.Length >= 2) {
                    port = int.Parse(args[1]);
                }
            }

            gameTypeCommand = GetGameTypeAndLoadEquip();
            connectAllUnconnectedRobots();
            
            return gameTypeCommand;
        }

        private static GameTypeCommand GetGameTypeAndLoadEquip() {
            ConnectionUtil connection = new ConnectionUtil();

            GameTypeCommand gameTypeCommand = connection.ConnectAsync(ip, port).Result;
            LoadEquip(connection.COMMUNICATION);
            return gameTypeCommand;
        }

        private static void connectAllUnconnectedRobots() {
            lock (ROBOT_COLLECTION) {
                Task[] tasks = new Task[ROBOT_COLLECTION.Count];
                int i = 0;
                foreach (var robot in ROBOT_COLLECTION) {
                    if (!robot.connected) {
                        tasks[i++] = robot.ConnectAsync(ip, port);
                    }
                }
                Task.WaitAll(tasks);
            }
        }

        /// <summary>
        /// End turn - every robots who do not send command will send command <code>Wait</code>.
        /// </summary>
        /// <seealso cref="Wait"/>
        public static void EndTurn() {
            lock (ROBOTS_TASK_COMMANDS) {
                lock (ROBOT_COLLECTION) {
                    foreach (var robot in ROBOT_COLLECTION) {
                        if (!ROBOTS_TASK_COMMANDS.ContainsKey(robot)) {
                            robot.Wait();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// How many lap will play.
        /// </summary>
        public int MAX_LAP { get; private set; }

        /// <summary>
        /// Number of actual lap.
        /// </summary>
        public int LAP { get; private set; }

        /// <summary>
        /// How many turn is maximally in one lap.
        /// </summary>
        public int MAX_TURN { get; private set; }

        /// <summary>
        /// Number of actual turn.
        /// </summary>
        public int TURN { get; private set; }
        
        /// <summary>
        /// Communication with server.
        /// </summary>
        private NetworkStream sns;

        /// <summary>
        /// Robot's name.
        /// </summary>
        private String name;

        /// <summary>
        /// Robot's team name.
        /// </summary>
        private String teamName;

        /// <summary>
        /// Flag if is robot already connected.
        /// </summary>
        private bool connected;

        /// <summary>
        /// Create new instance of robot. Robot's team name is <code>TEAM_NAME</code>
        /// </summary>
        /// <seealso cref="TEAM_NAME"/>
        /// <param name="name"> name of this robot</param>
        protected ClientRobot(String name) : this(name, TEAM_NAME) {}

        /// <summary>
        /// Create new instance of robot.
        /// </summary>
        /// <param name="name"> name of this robot</param>
        /// <param name="teamName">name of team</param>
        protected ClientRobot(String name, String teamName) {
            LAP = 1;
            this.name = name;
            this.teamName = teamName;

            lock (ROBOT_COLLECTION) {
                ROBOT_COLLECTION.Add(this);

                if (ip != null) {
                    ConnectAsync(ip, port).Wait();
                }
            }

        }

        /// <summary>
        /// Connect unconnected robot.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private async Task ConnectAsync(String ip, int port) {
            if (!connected) {
                ConnectionUtil connection = new ConnectionUtil();

                await connection.ConnectAsync(ip, port);
                sns = connection.COMMUNICATION;
                this.connected = true;
                await InitAsync(name, teamName);
            }
        }

        /// <summary>
        /// Set robot name and team name and send it to server asynchronously.
        /// </summary>
        /// <param name="name"> name of this robot</param>
        /// <param name="teamName">name of team (suggest to use <code>Guid.NewGuid().ToString();</code> for getting name</param>
        /// <returns></returns>
        private async Task<InitAnswerCommand> InitAsync(String name, String teamName) {
            await sendCommandAsync(new InitCommand(name, teamName, GetRobotType()));
            InitAnswerCommand answerCommand = (InitAnswerCommand) checkCommand(await sns.ReceiveCommandAsync());
            ProcessInit(answerCommand);

            RobotStateCommand robotState = (RobotStateCommand) checkCommand(await sns.ReceiveCommandAsync());
            ProcessState(robotState);
            return answerCommand;
        }

        /// <summary>
        /// What kind of robot it is.
        /// </summary>
        /// <seealso cref="RobotType"/>
        /// <returns></returns>
        public abstract RobotType GetRobotType();

        /// <summary>
        /// Get equipment from server.
        /// </summary>
        private static void LoadEquip(NetworkStream sns) {
            lock (MOTORS_BY_ID) {
                if (MOTORS_BY_ID.Count == 0) {
                    sns.SendCommandAsync(new GetMotorsCommand()).Wait();
                    GetMotorsAnswerCommand motorAnswer = (GetMotorsAnswerCommand) checkCommand(sns.ReceiveCommand());
                    sns.SendCommandAsync(new GetArmorsCommand()).Wait();
                    GetArmorsAnswerCommand armorsAnswer = (GetArmorsAnswerCommand)checkCommand(sns.ReceiveCommand());
                    sns.SendCommandAsync(new GetGunsCommand()).Wait();
                    GetGunsAnswerCommand gunAnswer = (GetGunsAnswerCommand) checkCommand(sns.ReceiveCommand());
                    sns.SendCommandAsync(new GetRepairToolsCommand()).Wait();
                    GetRepairToolsAnswerCommand repairToolsAnswer = (GetRepairToolsAnswerCommand) checkCommand(sns.ReceiveCommand());
                    sns.SendCommandAsync(new GetMineGunsCommand()).Wait();
                    GetMineGunsAnswerCommand mineGunsAnswer = (GetMineGunsAnswerCommand) checkCommand(sns.ReceiveCommand());

                    foreach (Motor motor in motorAnswer.MOTORS) {
                        MOTORS_BY_ID.Add(motor.ID, motor);
                    }

                    foreach (Armor armor in armorsAnswer.ARMORS) {
                        ARMORS_BY_ID.Add(armor.ID, armor);
                    }

                    foreach (Gun gun in gunAnswer.GUNS) {
                        GUNS_BY_ID.Add(gun.ID, gun);
                    }

                    foreach (RepairTool repairTool in repairToolsAnswer.REPAIR_TOOLS) {
                        REPAIR_TOOLS_BY_ID.Add(repairTool.ID, repairTool);
                    }

                    foreach (MineGun mineGun in mineGunsAnswer.MINE_GUNS) {
                        MINE_GUNS_BY_ID.Add(mineGun.ID, mineGun);
                    }

                }
            }
        }

        /// <summary>
        /// Check if command is not ErrorCommand
        /// </summary>
        /// <throws>NotSupportedException - if command is ErrorCommand and text is message from ErrorCommand</throws>
        private static T checkCommand<T>(T command) where T : ACommand {
            ErrorCommand error = command as ErrorCommand;
            if (error != null) {
                throw new NotSupportedException("ERROR:" + error.MESSAGE);
            }
            return command;
        }

        /// <summary>
        /// Set robot id and equipment
        /// </summary>
        /// <param name="init"></param>
        protected virtual void ProcessInit(InitAnswerCommand init) {
            this.ID = init.ROBOT_ID;
            this.Motor = MOTORS_BY_ID[init.MOTOR_ID];
            this.Armor = ARMORS_BY_ID[init.ARMOR_ID];
            MAX_LAP = init.MAX_LAP;
            TEAM_ID = init.TEAM_ID;
            MAX_TURN = init.MAX_TURN;
            SetClassEquip(init.CLASS_EQUIPMENT_ID);
        }

        /// <summary>
        /// Set class equipment to robot by id.
        /// </summary>
        /// <param name="id"></param>
        protected abstract void SetClassEquip(int id);

        /// <summary>
        /// What class equipment id is had by robot.
        /// </summary>
        /// <returns></returns>
        protected abstract IClassEquipment GetClassEquip();

        /// <summary>
        /// Set percentage power of motor and direction. At the end set <code>AngleDrive</code> if rotation success.
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="power">percentage from 0 to 100.</param>
        /// <seealso cref="AngleDrive"/>
        /// <returns></returns>
        public DriveAnswerCommand Drive(double angle, double power) {
            DriveAnswerCommand answer = new DriveAnswerCommand();
            addRobotTask(DriveAsync(answer, angle, power));
            return answer;
        }

        /// <summary>
        /// Set percentage power of motor and direction. It send this action to server asynchronously. At the end set <code>AngleDrive</code> if rotation success and fill answer data to <code>destination</code>.
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on. 12 hour in up.</param>
        /// <param name="power">
        public async Task DriveAsync(DriveAnswerCommand destination, double angle, double power) {
            await sendCommandAsync(new DriveCommand(power, angle));
            var answerCommand =  await receiveCommandAsync<DriveAnswerCommand>();
            if (answerCommand.SUCCESS) {
                AngleDrive = AngleUtils.NormalizeDegree(angle);
            }
            destination.FillData(answerCommand);
        }

        /// <summary>
        /// Robot make scan to angle and witch precision. That is mean if scanned robot is in angle to robot in range (angle - precision, angle+precision) it is success.
        /// </summary>
        /// <param name="angle">in degree. 0 is 3 hour. 90 is 6 hour and so on. 12 hour in up.</param>
        /// <param name="precision">parameter for sector. Min is 0 max is 10</param>
        public ScanAnswerCommand Scan(double angle, double precision) {
            ScanAnswerCommand answer = new ScanAnswerCommand();
            addRobotTask(ScanAsync(answer, angle, precision));
            return answer;
        }

        /// <summary>
        /// Robot make scan and send that action to server asynchronously. Answer data fill to <code>destination</code>.
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on. 12 hour in up.<</param>
        /// <param name="precision">parameter for sector. Min is 0 max is 10</param>
        private async Task ScanAsync(ScanAnswerCommand destination, double angle, double precision) {
            await sendCommandAsync(new ScanCommand(precision, angle));
            ScanAnswerCommand answer = await receiveCommandAsync<ScanAnswerCommand>();
            destination.FillData(answer);
        }


        /// <summary>
        /// Set x,y coordinates, hit points and power. Set score and gold and call <code>sendMerchant</code> if is end of lap
        /// </summary>
        /// <seealso cref="SendMerchant"/>
        /// <param name="state"></param>
        protected virtual void ProcessState(RobotStateCommand state) {
            this.X = state.X;
            this.Y = state.Y;
            this.HitPoints = state.HIT_POINTS;
            this.Power = state.POWER;
            this.TURN = state.TURN;
            if (state.END_LAP_COMMAND != null) {
                Gold = state.END_LAP_COMMAND.GOLD;
                Score = state.END_LAP_COMMAND.SCORE;
                
                if (LAP == MAX_LAP) {
                    Console.WriteLine("Match finish.");
                    Environment.Exit(0);
                }
                SendMerchant();
                LAP++;
            }
        }

        /// <summary>
        /// Robot will do nothing for this turn, but still move.
        /// </summary>
        public void Wait() {
            addRobotTask(WaitAsync());
        }

        /// <summary>
        /// Robot set action to do nothing and sent it to server asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task WaitAsync() {
            await sendCommandAsync(new WaitCommand());
        }

        /// <summary>
        /// Send request for buying motorId, armorId, classEquipmentId and repairHitPoints.
        /// </summary>
        /// <param name="motorId"></param>
        /// <param name="armorId"></param>
        /// <param name="classEquipmentId"></param>
        /// <param name="repairHitPoints"></param>
        /// <returns></returns>
        protected void Merchant(int motorId, int armorId, int classEquipmentId, int repairHitPoints) {
            sendCommandAsync(new MerchantCommand(motorId, armorId, classEquipmentId, repairHitPoints)).Wait();
            MerchantAnswerCommand answer = receiveCommandAsync<MerchantAnswerCommand>().Result;
            ProcessMerchant(answer);
        }

        /// <summary>
        /// Asynchronously send request for buying motorId, armorId, classEquipmentId and repairHitPoints.
        /// </summary>
        /// <param name="motorId"></param>
        /// <param name="armorId"></param>
        /// <param name="classEquipmentId"></param>
        /// <param name="repairHitPoints"></param>
        /// <returns></returns>
        private async Task MerchantAsync(int motorId, int armorId, int classEquipmentId, int repairHitPoints) {
            await sendCommandAsync(new MerchantCommand(motorId, armorId, classEquipmentId, repairHitPoints));
            MerchantAnswerCommand answer = await receiveCommandAsync<MerchantAnswerCommand>();
            ProcessMerchant(answer);
        }

        /// <summary>
        /// Setting what you want to buy at the end of lap. Use <code>Merchant</code> method for buying.
        /// </summary>
        /// <seealso cref="Merchant"/>
        protected virtual void SendMerchant() {
            Merchant(Motor.ID, Armor.ID, GetClassEquip().ID, HitPoints - Armor.MAX_HP);
        }

        /// <summary>
        /// Processing merchant answer. Set motor, armor and class equipment.
        /// </summary>
        /// <param name="merchantAnswer"></param>
        protected virtual void ProcessMerchant(MerchantAnswerCommand merchantAnswer) {
            this.Motor = MOTORS_BY_ID[merchantAnswer.MOTOR_ID_BOUGHT];
            this.Armor = ARMORS_BY_ID[merchantAnswer.ARMOR_ID_BOUGHT];
            SetClassEquip(merchantAnswer.CLASS_EQUIPMENT_ID_BOUGHT);
        }

        /// <summary>
        /// Asynchronously sending command.
        /// </summary>
        /// <param name="command">Instance of command witch you want to send.</param>
        /// <returns></returns>
        protected async Task sendCommandAsync(ACommand command) {
            await sns.SendCommandAsync(command);
        }

        /// <summary>
        /// Asynchronously receiving command from server.
        /// </summary>
        /// <returns>Command</returns>
        protected async Task<T> receiveCommandAsync<T>() where T : ACommand{
            T command = (T) checkCommand(await sns.ReceiveCommandAsync());
            ProcessState((RobotStateCommand) checkCommand(await sns.ReceiveCommandAsync()));
            return command;
        }

        /// <summary>
        /// Receiving command from server.
        /// </summary>
        /// <returns>Command</returns>
        protected T receiveCommand<T>() where T : ACommand {
            T command = receiveCommandAsync<T>().Result;
            return checkCommand(command);
        }

        /// <summary>
        /// Add robot task, so ClientRobot class know when all robot have prepared command.
        /// </summary>
        /// <param name="t"></param>
        /// <throws>NotSupportedException - when robot is not connected or when robot already select action for this turn.</throws>
        protected void addRobotTask(Task t) {
            if (!connected) {
                throw new NotSupportedException("Robots have to be connected. Use ClientRobot.Connect(args) first.");
            }
            
            lock (ROBOTS_TASK_COMMANDS) {
                if (ROBOTS_TASK_COMMANDS.ContainsKey(this)) {
                    throw new NotSupportedException("Robot can not send more then one command during same turn.");
                }
                ROBOTS_TASK_COMMANDS.Add(this, t);
                lock (ROBOT_COLLECTION) {
                    if (ROBOTS_TASK_COMMANDS.Count == ROBOT_COLLECTION.Count) {
                        Task.WaitAll(ROBOTS_TASK_COMMANDS.Values.ToArray());
                        ROBOTS_TASK_COMMANDS.Clear();
                    }
                }
            }
        }


        static void MyHandler(object sender, UnhandledExceptionEventArgs e) {
            Console.Error.WriteLine(DateTime.Now);
            Console.Error.WriteLine(e.ExceptionObject);
            Console.Error.Flush();
            if (e.ExceptionObject is Exception ex) {
                Console.WriteLine("Some error occurs:'" + ex.Message + "'. Application store more information in " + ERROR_FILE_NAME + " and will be closed.");
            } else {
                Console.WriteLine("Some error occurs. Application store more information in " + ERROR_FILE_NAME + " and will be closed.");
            }
            Console.WriteLine("Continue with pressing eny key.");
            Console.Read();
            Environment.Exit(1);
        }
    }
}
