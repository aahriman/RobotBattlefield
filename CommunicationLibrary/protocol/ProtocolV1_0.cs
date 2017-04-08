using CommunicationLibrary.command.v1._0;

namespace CommunicationLibrary.protocol {
    public class ProtocolV1_0 : AProtocol{
        
        public ProtocolV1_0() : base(){
			comandsFactory.registerCommand(InitCommandV1_0.FACTORY);
			comandsFactory.registerCommand(InitAnswerCommandV1_0.FACTORY);

            comandsFactory.registerCommand(WaitCommandV1_0.FACTORY);

            comandsFactory.registerCommand(ShotCommandV1_0.FACTORY);
            comandsFactory.registerCommand(ShotAnswerCommandV1_0.FACTORY);

            comandsFactory.registerCommand(DriveCommandV1_0.FACTORY);
            comandsFactory.registerCommand(DriveAnswerCommandV1_0.FACTORY);

            comandsFactory.registerCommand(ScanCommandV1_0.FACTORY);
            comandsFactory.registerCommand(ScanAnswerCommandV1_0.FACTORY);

            comandsFactory.registerCommand(RobotStateCommandV1_0.FACTORY);

			comandsFactory.registerCommand(EndLapCommandV1_0.FACTORY);

			comandsFactory.registerCommand(EndLapCommandV1_0.FACTORY);

            comandsFactory.registerCommand(GetGunsCommandV1_0.FACTORY);
            comandsFactory.registerCommand(GetGunsAnwerCommandV1_0.FACTORY);

            comandsFactory.registerCommand(GetArmorsCommandV1_0.FACTORY);
            comandsFactory.registerCommand(GetArmorsAnwerCommandV1_0.FACTORY);

            comandsFactory.registerCommand(GetMotorsCommandV1_0.FACTORY);
			comandsFactory.registerCommand(GetMotorsAnwerCommandV1_0.FACTORY);

			comandsFactory.registerCommand(MerchantAnswerCommandV1_0.FACTORY);
			comandsFactory.registerCommand(MerchantCommandV1_0.FACTORY);
        }
    }
}
