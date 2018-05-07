using BaseLibrary.command.miner;
using BaseLibrary.command.repairman;
using BaseLibrary.command.v1._0;
using BaseLibrary.command.v1._0.equipment;
using BaseLibrary.command.v1._0.tank;

namespace BaseLibrary.protocol {
    /// <summary>
    /// Implementation of Protocol v1.0 (commands class are in command/v1_0
    /// </summary>
    [ProtocolDescription("v1.0")]
    public class ProtocolV1_0 : AProtocol{
        
        public ProtocolV1_0() : base(){
            commandsFactory.RegisterCommand(GameTypeCommandV1_0.FACTORY);

			commandsFactory.RegisterCommand(InitCommandV1_0.FACTORY);
			commandsFactory.RegisterCommand(InitAnswerCommandV1_0.FACTORY);

            commandsFactory.RegisterCommand(WaitCommandV1_0.FACTORY);

            commandsFactory.RegisterCommand(DriveCommandV1_0.FACTORY);
            commandsFactory.RegisterCommand(DriveAnswerCommandV1_0.FACTORY);

            commandsFactory.RegisterCommand(ScanCommandV1_0.FACTORY);
            commandsFactory.RegisterCommand(ScanAnswerCommandV1_0.FACTORY);

            commandsFactory.RegisterCommand(RobotStateCommandV1_0.FACTORY);

			commandsFactory.RegisterCommand(EndLapCommandV1_0.FACTORY);

			commandsFactory.RegisterCommand(EndLapCommandV1_0.FACTORY);

            commandsFactory.RegisterCommand(GetGunsCommandV1_0.FACTORY);
            commandsFactory.RegisterCommand(GetGunsAnswerCommandV1_0.FACTORY);

            commandsFactory.RegisterCommand(GetMineGunsCommandV10.FACTORY);
            commandsFactory.RegisterCommand(GetMineGunsAnswerCommandV10.FACTORY);

            commandsFactory.RegisterCommand(GetRepairToolsCommandV10.FACTORY);
            commandsFactory.RegisterCommand(GetRepairToolsAnswerCommandV10.FACTORY);

            commandsFactory.RegisterCommand(GetArmorsCommandV1_0.FACTORY);
            commandsFactory.RegisterCommand(GetArmorsAnswerCommandV10.FACTORY);

            commandsFactory.RegisterCommand(GetMotorsCommandV1_0.FACTORY);
			commandsFactory.RegisterCommand(GetMotorsAnswerCommandV1_0.FACTORY);

			commandsFactory.RegisterCommand(MerchantAnswerCommandV1_0.FACTORY);
			commandsFactory.RegisterCommand(MerchantCommandV1_0.FACTORY);


            // TANK COMMANDS

            commandsFactory.RegisterCommand(ShootCommandV10.FACTORY);
            commandsFactory.RegisterCommand(ShootAnswerCommandV10.FACTORY);

            // MINE_LAYER COMMANDS
            commandsFactory.RegisterCommand(PutMineCommandV1_0.FACTORY);
            commandsFactory.RegisterCommand(PutMineAnswerCommandV1_0.FACTORY);

            commandsFactory.RegisterCommand(DetonateMineCommandV1_0.FACTORY);
            commandsFactory.RegisterCommand(DetonateMineAnswerCommandV1_0.FACTORY);

            // REPAIRMAN COMMANDS
            commandsFactory.RegisterCommand(RepairCommandV1_0.FACTORY);
            commandsFactory.RegisterCommand(RepairAnswerCommandV1_0.FACTORY);
        }
    }
}
