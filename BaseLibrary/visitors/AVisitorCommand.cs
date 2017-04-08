using BaseLibrary.command;

namespace BaseLibrary.visitors {
    public abstract class AVisitorCommand {


        public abstract void visit(GetArmorsCommand visitor);

        public abstract void visit(GetArmorsAnwerCommand visitor);

        public abstract void visit(GetGunsCommand visitor);

        public abstract void visit(GetGunsAnwerCommand visitor);

        public abstract void visit(GetMotorsCommand visitor);

        public abstract void visit(GetMotorsAnwerCommand visitor);

        public abstract void visit(InitCommand visitor);

        public abstract void visit(InitAnswerCommand visitor);

        public abstract void visit(DriveCommand visitor);

        public abstract void visit(DriveAnswerCommand visitor);

        public abstract void visit(RobotStateCommand visitor);

        public abstract void visit(ScanCommand visitor);

        public abstract void visit(ScanAnswerCommand visitor);

        public abstract void visit(ShotCommand visitor);

        public abstract void visit(ShotAnswerCommand visitor);

        public abstract void visit(WaitCommand visitor);

        public abstract void visit(EndLapCommand visitor);

        public abstract void visit(MerchantCommand visitor);

        public abstract void visit(MerchantAnswerCommand visitor);

        public abstract void visit(EndMatchCommand visitor);
    }

    public abstract class AVisitorCommand<Output> {

        public abstract Output visit(GetArmorsCommand visitor);

        public abstract Output visit(GetArmorsAnwerCommand visitor);

        public abstract Output visit(GetGunsCommand visitor);

        public abstract Output visit(GetGunsAnwerCommand visitor);

        public abstract Output visit(GetMotorsCommand visitor);

        public abstract Output visit(GetMotorsAnwerCommand visitor);

        public abstract Output visit(InitCommand visitor);

        public abstract Output visit(InitAnswerCommand visitor);

        public abstract Output visit(DriveCommand visitor);

        public abstract Output visit(DriveAnswerCommand visitor);

        public abstract Output visit(RobotStateCommand visitor);

        public abstract Output visit(ScanCommand visitor);

        public abstract Output visit(ScanAnswerCommand visitor);

        public abstract Output visit(ShotCommand visitor);

        public abstract Output visit(ShotAnswerCommand visitor);

        public abstract Output visit(WaitCommand visitor);

        public abstract Output visit(EndLapCommand visitor);

        public abstract Output visit(MerchantCommand visitor);

        public abstract Output visit(MerchantAnswerCommand visitor);

        public abstract Output visit(EndMatchCommand visitor);
    }

    public abstract class AVisitorCommand<Output, Input> {

        public abstract Output visit(GetArmorsCommand visitor, params Input[] inputs);

        public abstract Output visit(GetArmorsAnwerCommand visitor, params Input[] inputs);

        public abstract Output visit(GetGunsCommand visitor, params Input[] inputs);

        public abstract Output visit(GetGunsAnwerCommand visitor, params Input[] inputs);

        public abstract Output visit(GetMotorsCommand visitor, params Input[] inputs);

        public abstract Output visit(GetMotorsAnwerCommand visitor, params Input[] inputs);

        public abstract Output visit(InitCommand visitor, params Input[] inputs);

        public abstract Output visit(InitAnswerCommand visitor, params Input[] inputs);

        public abstract Output visit(DriveCommand visitor, params Input[] inputs);

        public abstract Output visit(DriveAnswerCommand visitor, params Input[] inputs);

        public abstract Output visit(RobotStateCommand visitor, params Input[] inputs);

        public abstract Output visit(ScanCommand visitor, params Input[] inputs);

        public abstract Output visit(ScanAnswerCommand visitor, params Input[] inputs);

        public abstract Output visit(ShotCommand visitor, params Input[] inputs);

        public abstract Output visit(ShotAnswerCommand visitor, params Input[] inputs);

        public abstract Output visit(WaitCommand visitor, params Input[] inputs);

        public abstract Output visit(EndLapCommand visitor, params Input[] inputs);

        public abstract Output visit(MerchantCommand visitor, params Input[] inputs);

        public abstract Output visit(MerchantAnswerCommand visitor, params Input[] inputs);

        public abstract Output visit(EndMatchCommand visitor, params Input[] inputs);
    }

	internal class NullType { }

}
