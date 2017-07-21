using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLibrary;
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
    public abstract class ClientRobot : AClientRobot {
        static ClientRobot() {
            ModUtils.LoadMods();
        }

        private static readonly Object EQUIP_LOCK = new Object();
        public static readonly Dictionary<int, Motor> MOTORS_BY_ID = new Dictionary<int, Motor>();
        public static readonly Dictionary<int, Armor> ARMORS_BY_ID = new Dictionary<int, Armor>();
        public static readonly Dictionary<int, Gun> GUNS_BY_ID = new Dictionary<int, Gun>();
        public static readonly Dictionary<int, RepairTool> REPAIR_TOOLS_BY_ID = new Dictionary<int, RepairTool>();
        public static readonly Dictionary<int, MineGun> MINE_GUNS_BY_ID = new Dictionary<int, MineGun>();

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

        protected bool processStateAfterEveryCommand;
        protected bool processMerchant;
        protected ClientRobot() : this(true, true) {}

        protected ClientRobot(bool processStateAfterEveryCommand, bool processMerchant) {
            this.processStateAfterEveryCommand = processStateAfterEveryCommand;
            this.processMerchant = processMerchant;
            LAP = 1;
        }

        public GameTypeCommand Connect(String [] args) {
            String ip = AClientRobot.LOCAL_ADDRES;
            int port = GameProperties.DEFAULT_PORT;
            if (args.Length >= 1) {
                ip = args[0];
            }

            if (args.Length >= 2) {
                port = int.Parse(args[1]);
            }

            GameTypeCommand gameTypeCommand = taskWait(ConnectAsync(ip, port));
            afterConnect();
            return gameTypeCommand;
        }

        public GameTypeCommand Connect() {
            GameTypeCommand gameTypeCommand = taskWait(ConnectAsync());
            afterConnect();
            return gameTypeCommand;
        }

        public GameTypeCommand Connect(int port) {
            GameTypeCommand gameTypeCommand = taskWait(ConnectAsync(port));
            afterConnect();
            return gameTypeCommand;
        }

        public GameTypeCommand Connect(String ip, int port) {
            GameTypeCommand gameTypeCommand = taskWait(ConnectAsync(ip, port));
            afterConnect();
            return gameTypeCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"> name of this robot</param>
        /// <param name="teamName">name of team (suggest to use <code>Guid.NewGuid().ToString();</code> for getting name</param>
        /// <returns></returns>
        public InitAnswerCommand Init(String name, String teamName) {
            InitAnswerCommand answer = taskWait(InitAsync(name, teamName));
            return answer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"> name of this robot</param>
        /// <param name="teamName">name of team (suggest to use <code>Guid.NewGuid().ToString();</code> for getting name</param>
        /// <returns></returns>
        public async Task<InitAnswerCommand> InitAsync(String name, String teamName) {
            await sendCommandAsync(new InitCommand(name, teamName, GetRobotType()));
            var answerCommand =  (InitAnswerCommand)await recieveCommandAsync();
            ProcessInit(answerCommand);
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        public abstract RobotType GetRobotType();

        public void LoadEquip() {
            lock (EQUIP_LOCK) {
                if (MOTORS_BY_ID.Count == 0) {
                    Task.WaitAll(sendCommandAsync(new GetMotorsCommand()));
                    GetMotorsAnswerCommand motorAnswer = (GetMotorsAnswerCommand) taskWait(recieveCommandAsync());
                    Task.WaitAll(sendCommandAsync(new GetArmorsCommand()));
                    GetArmorsAnswerCommand armorsAnswer = (GetArmorsAnswerCommand) taskWait(recieveCommandAsync());
                    Task.WaitAll(sendCommandAsync(new GetGunsCommand()));
                    GetGunsAnswerCommand gunAnswer = (GetGunsAnswerCommand) taskWait(recieveCommandAsync());
                    Task.WaitAll(sendCommandAsync(new GetRepairToolCommand()));
                    GetRepairToolAnswerCommand repairToolAnswer = (GetRepairToolAnswerCommand) taskWait(recieveCommandAsync());
                    Task.WaitAll(sendCommandAsync(new GetMineGunCommand()));
                    GetMineGunAnswerCommand mineGunAnswer = (GetMineGunAnswerCommand) taskWait(recieveCommandAsync());

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
        public virtual void ProcessInit(InitAnswerCommand init) {
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
            var answerCommand =  (DriveAnswerCommand)await recieveCommandAsync();
            if (answerCommand.SUCCES) {
                AngleDrive = AngleUtils.NormalizeDegree(angle);
            }
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
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
            var answerCommand =  (ScanAnswerCommand)await recieveCommandAsync();
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        public RobotStateCommand State() {
            RobotStateCommand answer = taskWait(StateAsync());
            return answer;
        }

        public async Task<RobotStateCommand> StateAsync() {
            return (RobotStateCommand)await recieveCommandAsync();
        }

        /// <summary>
        /// Set x,y coordinates, hit points and power. Set score and gold and repair if is end of lap. 
        /// </summary>
        /// <param name="state"></param>
        public virtual void ProcessState(RobotStateCommand state) {
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
                if (processMerchant) {
                    ProcessMerchant(Merchant(Motor.ID, Armor.ID, getClassEquip().ID, 100));
                }
                LAP++;
            }
        }

        public void Wait() {
            taskWait(WaitAsync());
        }

        public async Task WaitAsync() {
            await sendCommandAsync(new WaitCommand());
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
        }

        public MerchantAnswerCommand Merchant(int motorId, int armorId, int classEquipmentId, int repairHP) {
            MerchantAnswerCommand answer = taskWait(MercantAsync(motorId, armorId, classEquipmentId, repairHP));
            return answer;
        }

        public async Task<MerchantAnswerCommand> MercantAsync(int motorId, int armorId, int classEquipmentId, int repairHP) {
            await sendCommandAsync(new MerchantCommand(motorId, armorId, classEquipmentId, repairHP));
            var answerCommand =  (MerchantAnswerCommand)await recieveCommandAsync();
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        public virtual void ProcessMerchant(MerchantAnswerCommand merchantAnswer) {
            this.Motor = MOTORS_BY_ID[merchantAnswer.MOTOR_ID_BOUGHT];
            this.Armor = ARMORS_BY_ID[merchantAnswer.ARMOR_ID_BOUGHT];
            setClassEquip(merchantAnswer.CLASS_EQUIPMENT_ID_BOUGHT);
        }

        protected async Task sendCommandAsync(ACommand command) {
            await sns.SendCommandAsync(command);
        }

        protected async Task<ACommand> recieveCommandAsync() {
            return await sns.RecieveCommandAsync();
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

        /// <summary>
        /// Specify what to do at the end of Connection methods
        /// </summary>
        protected void afterConnect() {
            LoadEquip();
        }
    }
}
