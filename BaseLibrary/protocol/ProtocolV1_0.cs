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
            comandsFactory.RegisterCommand(GameTypeCommandV1_0.FACTORY);

			comandsFactory.RegisterCommand(InitCommandV1_0.FACTORY);
			comandsFactory.RegisterCommand(InitAnswerCommandV1_0.FACTORY);

            comandsFactory.RegisterCommand(WaitCommandV1_0.FACTORY);

            comandsFactory.RegisterCommand(DriveCommandV1_0.FACTORY);
            comandsFactory.RegisterCommand(DriveAnswerCommandV1_0.FACTORY);

            comandsFactory.RegisterCommand(ScanCommandV1_0.FACTORY);
            comandsFactory.RegisterCommand(ScanAnswerCommandV1_0.FACTORY);

            comandsFactory.RegisterCommand(RobotStateCommandV1_0.FACTORY);

			comandsFactory.RegisterCommand(EndLapCommandV1_0.FACTORY);

			comandsFactory.RegisterCommand(EndLapCommandV1_0.FACTORY);

            comandsFactory.RegisterCommand(GetGunsCommandV1_0.FACTORY);
            comandsFactory.RegisterCommand(GetGunsAnswerCommandV1_0.FACTORY);

            comandsFactory.RegisterCommand(GetMineGunsCommandV10.FACTORY);
            comandsFactory.RegisterCommand(GetMineGunsAnswerCommandV10.FACTORY);

            comandsFactory.RegisterCommand(GetRepairToolsCommandV10.FACTORY);
            comandsFactory.RegisterCommand(GetRepairToolsAnswerCommandV10.FACTORY);

            comandsFactory.RegisterCommand(GetArmorsCommandV1_0.FACTORY);
            comandsFactory.RegisterCommand(GetArmorsAnswerCommandV10.FACTORY);

            comandsFactory.RegisterCommand(GetMotorsCommandV1_0.FACTORY);
			comandsFactory.RegisterCommand(GetMotorsAnswerCommandV1_0.FACTORY);

			comandsFactory.RegisterCommand(MerchantAnswerCommandV1_0.FACTORY);
			comandsFactory.RegisterCommand(MerchantCommandV1_0.FACTORY);


            // TANK COMMANDS

            comandsFactory.RegisterCommand(ShootCommandV10.FACTORY);
            comandsFactory.RegisterCommand(ShootAnswerCommandV10.FACTORY);

            // MINE_LAYER COMMANDS
            comandsFactory.RegisterCommand(PutMineCommandV1_0.FACTORY);
            comandsFactory.RegisterCommand(PutMineAnswerCommandV1_0.FACTORY);

            comandsFactory.RegisterCommand(DetonateMineCommandV1_0.FACTORY);
            comandsFactory.RegisterCommand(DetonateMineAnswerCommandV1_0.FACTORY);

            // REPAIRMAN COMMANDS
            comandsFactory.RegisterCommand(RepairCommandV1_0.FACTORY);
            comandsFactory.RegisterCommand(RepairAnswerCommandV1_0.FACTORY);
        }
    }
}
