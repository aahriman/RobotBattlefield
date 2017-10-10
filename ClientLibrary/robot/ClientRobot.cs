using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.equipment;
using BaseLibrary.command.handshake;
using BaseLibrary.config;
using BaseLibrary.equip;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using ClientLibrary.config;

namespace ClientLibrary.robot {
    public abstract class ClientRobot : Robot {

        public static readonly String TEAM_NAME = new Guid().ToString();

        private static readonly List<ClientRobot> ROBOT_COLLECTION = new List<ClientRobot>();
        private static readonly IDictionary<ClientRobot, Task> ROBOTS_TASK_COMMANDS = new Dictionary<ClientRobot, Task>();

        private static String ip = null;
        private static int port;

        static ClientRobot() {
            ModUtils.LoadMods();
        }

        private static readonly Object EQUIP_LOCK = new Object();
        public static readonly Dictionary<int, Motor> MOTORS_BY_ID = new Dictionary<int, Motor>();
        public static readonly Dictionary<int, Armor> ARMORS_BY_ID = new Dictionary<int, Armor>();
        public static readonly Dictionary<int, Gun> GUNS_BY_ID = new Dictionary<int, Gun>();
        public static readonly Dictionary<int, RepairTool> REPAIR_TOOLS_BY_ID = new Dictionary<int, RepairTool>();
        public static readonly Dictionary<int, MineGun> MINE_GUNS_BY_ID = new Dictionary<int, MineGun>();

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
            ip = ConnectionUtil.LOCAL_ADDRES;
            port = GameProperties.DEFAULT_PORT;
            if (args.Length >= 1) {
                ip = args[0];
            }

            if (args.Length >= 2) {
                port = int.Parse(args[1]);
            }

            GameTypeCommand gameTypeCommand = null;
            lock (ROBOT_COLLECTION) {
                foreach (var robot in ROBOT_COLLECTION) {
                    if (!robot.connected) {
                        gameTypeCommand = robot.Connect(ip, port);
                    }
                }
            }
            return gameTypeCommand;
        }


        /// <summary>
        /// End turn - every robots who do not send command will send command <code>Wait</code>
        /// </summary>
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

        public int MAX_LAP { get; private set; }
        public int MAX_TURN { get; private set; }
        public int LAP { get; private set; }
        public String NAME { get; private set; }

        public override int HitPoints { get; set; }
        public override int Score { get; set; }
        public override int Gold { get; set; }
        public override double X { get; set; }
        public override double Y { get; set; }
        public override double Power { get; set; }
        public override double AngleDrive { get; set; }
        public override Motor Motor { get; set; }
        public override Armor Armor { get; set; }


        private SuperNetworkStream sns;

        private String name;
        private String teamName;
        private bool connected;

        protected ClientRobot(String name) : this(name, TEAM_NAME) {}

        protected ClientRobot(String name, String teamName) {
            lock (ROBOT_COLLECTION) {
                LAP = 1;
                this.name = name;
                this.teamName = teamName;

                if (ip != null) {
                    Connect(ip, port);
                }

                ROBOT_COLLECTION.Add(this);
            }
        }

        private GameTypeCommand Connect(String ip, int port) {
            GameTypeCommand gameTypeCommand = null;
            if (connected) {
                ConnectionUtil connection = new ConnectionUtil();

                gameTypeCommand = taskWait(connection.ConnectAsync(ip, port));
                sns = connection.COMMUNICATION;
                afterConnect();
                this.connected = true;
                Init(name, teamName);
            }
            return gameTypeCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"> name of this robot</param>
        /// <param name="teamName">name of team (suggest to use <code>Guid.NewGuid().ToString();</code> for getting name</param>
        /// <returns></returns>
        private InitAnswerCommand Init(String name, String teamName) {
            InitAnswerCommand answer = taskWait(InitAsync(name, teamName));
            return answer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"> name of this robot</param>
        /// <param name="teamName">name of team (suggest to use <code>Guid.NewGuid().ToString();</code> for getting name</param>
        /// <returns></returns>
        private async Task<InitAnswerCommand> InitAsync(String name, String teamName) {
            await sendCommandAsync(new InitCommand(name, teamName, GetRobotType()));
            var answerCommand = await recieveCommandAsync<InitAnswerCommand>();
            ProcessInit(answerCommand);
            return answerCommand;
        }

        public abstract RobotType GetRobotType();

        public void LoadEquip() {
            lock (EQUIP_LOCK) {
                if (MOTORS_BY_ID.Count == 0) {
                    Task.WaitAll(sendCommandAsync(new GetMotorsCommand()));
                    GetMotorsAnswerCommand motorAnswer = recieveCommand<GetMotorsAnswerCommand>();
                    Task.WaitAll(sendCommandAsync(new GetArmorsCommand()));
                    GetArmorsAnswerCommand armorsAnswer = recieveCommand<GetArmorsAnswerCommand>();
                    Task.WaitAll(sendCommandAsync(new GetGunsCommand()));
                    GetGunsAnswerCommand gunAnswer = recieveCommand<GetGunsAnswerCommand>();
                    Task.WaitAll(sendCommandAsync(new GetRepairToolCommand()));
                    GetRepairToolAnswerCommand repairToolAnswer = recieveCommand<GetRepairToolAnswerCommand>();
                    Task.WaitAll(sendCommandAsync(new GetMineGunCommand()));
                    GetMineGunAnswerCommand mineGunAnswer = recieveCommand<GetMineGunAnswerCommand>();

                    foreach (var motor in motorAnswer.MOTORS) {
                        MOTORS_BY_ID.Add(motor.ID, motor);
                    }

                    foreach (var armor in armorsAnswer.ARMORS) {
                        ARMORS_BY_ID.Add(armor.ID, armor);
                    }

                    foreach (var gun in gunAnswer.GUNS) {
                        GUNS_BY_ID.Add(gun.ID, gun);
                    }

                    foreach (var repairTool in repairToolAnswer.REPAIR_TOOLS) {
                        REPAIR_TOOLS_BY_ID.Add(repairTool.ID, repairTool);
                    }

                    foreach (var mineGun in mineGunAnswer.MINE_GUNS) {
                        MINE_GUNS_BY_ID.Add(mineGun.ID, mineGun);
                    }

                }
            }
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
            setClassEquip(init.CLASS_EQUIPMENT_ID);
        }

        protected abstract void setClassEquip(int id);
        protected abstract ClassEquipment getClassEquip();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="power">percentage from 0 to 100.</param>
        /// <returns></returns>
        public DriveAnswerCommand Drive(double angle, double power) {
            DriveAnswerCommand answer = taskWait(DriveAsync(angle, power));
            return answer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="power">
        public async Task<DriveAnswerCommand> DriveAsync(double angle, double power) {
            await sendCommandAsync(new DriveCommand(power, angle));
            var answerCommand =  await recieveCommandAsync<DriveAnswerCommand>();
            if (answerCommand.SUCCES) {
                AngleDrive = AngleUtils.NormalizeDegree(angle);
            }
            return answerCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="precision">parameter for sector. Min is 0 max is 10</param>
        public ScanAnswerCommand Scan(double angle, double precision) {
            ScanAnswerCommand answer = taskWait(ScanAsync(angle, precision));
            return answer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="precision">parameter for sector. Min is 0 max is 10</param>
        public async Task<ScanAnswerCommand> ScanAsync(double angle, double precision) {
            await sendCommandAsync(new ScanCommand(precision, angle));
            return await recieveCommandAsync<ScanAnswerCommand>();
        }


        /// <summary>
        /// Set x,y coordinates, hit points and power. Set score and gold and repair if is end of lap. 
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ProcessState(RobotStateCommand state) {
            this.X = state.X;
            this.Y = state.Y;
            this.HitPoints = state.HIT_POINTS;
            this.Power = state.POWER;
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
        /// Robot will do nothing for this turn. He still move, but not change direction or wanted power (speed).
        /// </summary>
        public void Wait() {
            addRobotTask(WaitAsync());
        }

        private async Task WaitAsync() {
            await sendCommandAsync(new WaitCommand());
        }

        protected MerchantAnswerCommand Merchant(int motorId, int armorId, int classEquipmentId, int repairHitPoints) {
            MerchantAnswerCommand answer = new MerchantAnswerCommand();
            addRobotTask(MercantAsync(answer, motorId, armorId, classEquipmentId, repairHitPoints));
            return answer;
        }

        private async Task MercantAsync(MerchantAnswerCommand destination, int motorId, int armorId, int classEquipmentId, int repairHitPoints) {
            await sendCommandAsync(new MerchantCommand(motorId, armorId, classEquipmentId, repairHitPoints));
            MerchantAnswerCommand answer = await recieveCommandAsync<MerchantAnswerCommand>();
            ProcessMerchant(answer);
            destination.FillData(answer);
        }

        protected virtual void SendMerchant() {
            Merchant(Motor.ID, Armor.ID, getClassEquip().ID, 100);
        }

        protected virtual void ProcessMerchant(MerchantAnswerCommand merchantAnswer) {
            this.Motor = MOTORS_BY_ID[merchantAnswer.MOTOR_ID_BOUGHT];
            this.Armor = ARMORS_BY_ID[merchantAnswer.ARMOR_ID_BOUGHT];
            setClassEquip(merchantAnswer.CLASS_EQUIPMENT_ID_BOUGHT);
        }

        protected async Task sendCommandAsync(ACommand command) {
            await sns.SendCommandAsync(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task<T> recieveCommandAsync<T>() where T : ACommand{
            T command = (T) await sns.RecieveCommandAsync();
            ProcessState((RobotStateCommand) await sns.RecieveCommandAsync());
            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected T recieveCommand<T>() where T : ACommand {
            T command = recieveCommandAsync<T>().Result;
            return command;
        }


        /// <summary>
        /// Wait for task and if his InnerException is not null it throw it else it throw AggregateException or nothing if no exception occures.
        /// </summary>
        /// <exception cref="AggregateException">If waited task throw <code>AggregateException</code> and dont have any InnerException it throw it</exception>
        /// <param name="task"></param>
        protected void taskWait(Task task) {
            try {
                Task.WaitAll(task);
            } catch (AggregateException e) {
                if (e.InnerException != null) {
                    throw e.InnerException;
                } else {
                    throw e;
                }
            }
        }


        /// <summary>
        /// Wait for task and if his InnerException is not null it throw it else it throw AggregateException. If no exception occured it throw <code>TResult</code>
        /// </summary>
        /// <exception cref="AggregateException">If waited task throw <code>AggregateException</code> and dont have any InnerException it throw it</exception>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        protected TResult taskWait<TResult>(Task<TResult> task) {
            try {
                Task.WaitAll(task);
                return task.Result;
            } catch (AggregateException e) {
                if (e.InnerException != null) {
                    throw e.InnerException;
                } else {
                    throw e;
                }
            }
        }

        protected void addRobotTask(Task t) {
            lock (ROBOTS_TASK_COMMANDS) {
                ROBOTS_TASK_COMMANDS.Add(this, t);
                lock (ROBOT_COLLECTION) {
                    if (ROBOTS_TASK_COMMANDS.Count == ROBOT_COLLECTION.Count) {
                        Task.WaitAll(ROBOTS_TASK_COMMANDS.Values.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Specify what to do at the end of Connection methods
        /// </summary>
        protected void afterConnect() {
            LoadEquip();
        }
    }
}
