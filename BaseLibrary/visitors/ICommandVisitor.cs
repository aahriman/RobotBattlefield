using BaseLibrary.command;
using BaseLibrary.command.equipment;

namespace BaseLibrary.visitors {
    public interface ICommandVisitor {

        void visit(GetArmorsCommand visitor);
        void visit(GetArmorsAnswerCommand visitor);

        void visit(GetMotorsCommand visitor);
        void visit(GetMotorsAnswerCommand visitor);

        void visit(InitCommand visitor);
        void visit(InitAnswerCommand visitor);

        void visit(DriveCommand visitor);
        void visit(DriveAnswerCommand visitor);

        void visit(RobotStateCommand visitor);

        void visit(ScanCommand visitor);
        void visit(ScanAnswerCommand visitor);

        void visit(WaitCommand visitor);

        void visit(EndLapCommand visitor);

        void visit(MerchantCommand visitor);
        void visit(MerchantAnswerCommand visitor);

        void visit(EndMatchCommand visitor);

        void visit(GameTypeCommand visitor);

        void visit(GetGunsCommand element);
        void visit(GetGunsAnswerCommand element);

        void visit(GetRepairToolCommand element);
        void visit(GetRepairToolAnswerCommand element);

        void visit(GetMineGunCommand element);
        void visit(GetMineGunAnswerCommand element);

    }

    public interface ICommandVisitor<out Output> {

        Output visit(GetArmorsCommand visitor);
        Output visit(GetArmorsAnswerCommand visitor);

        Output visit(GetMotorsCommand visitor);
        Output visit(GetMotorsAnswerCommand visitor);

        Output visit(InitCommand visitor);
        Output visit(InitAnswerCommand visitor);

        Output visit(DriveCommand visitor);
        Output visit(DriveAnswerCommand visitor);

        Output visit(RobotStateCommand visitor);

        Output visit(ScanCommand visitor);
        Output visit(ScanAnswerCommand visitor);

        Output visit(WaitCommand visitor);

        Output visit(EndLapCommand visitor);

        Output visit(MerchantCommand visitor);
        Output visit(MerchantAnswerCommand visitor);

        Output visit(EndMatchCommand visitor);

        Output visit(GameTypeCommand visitor);

        Output visit(GetGunsCommand element);
        Output visit(GetGunsAnswerCommand element);

        Output visit(GetRepairToolCommand element);
        Output visit(GetRepairToolAnswerCommand element);

        Output visit(GetMineGunCommand element);
        Output visit(GetMineGunAnswerCommand element);
    }

    public interface ICommandVisitor<out Output, in Input> {

        Output visit(GetArmorsCommand visitor, params Input[] inputs);
        Output visit(GetArmorsAnswerCommand visitor, params Input[] inputs);

        Output visit(GetMotorsCommand visitor, params Input[] inputs);
        Output visit(GetMotorsAnswerCommand visitor, params Input[] inputs);

        Output visit(InitCommand visitor, params Input[] inputs);
        Output visit(InitAnswerCommand visitor, params Input[] inputs);

        Output visit(DriveCommand visitor, params Input[] inputs);
        Output visit(DriveAnswerCommand visitor, params Input[] inputs);

        Output visit(RobotStateCommand visitor, params Input[] inputs);

        Output visit(ScanCommand visitor, params Input[] inputs);
        Output visit(ScanAnswerCommand visitor, params Input[] inputs);

        Output visit(WaitCommand visitor, params Input[] inputs);

        Output visit(EndLapCommand visitor, params Input[] inputs);

        Output visit(MerchantCommand visitor, params Input[] inputs);
        Output visit(MerchantAnswerCommand visitor, params Input[] inputs);

        Output visit(EndMatchCommand visitor, params Input[] inputs);

        Output visit(GameTypeCommand visitor, params Input[] inputs);

        Output visit(GetGunsCommand element, params Input[] inputs);
        Output visit(GetGunsAnswerCommand element, params Input[] inputs);

        Output visit(GetRepairToolCommand element, params Input[] inputs);
        Output visit(GetRepairToolAnswerCommand element, params Input[] inputs);

        Output visit(GetMineGunCommand element, params Input[] inputs);
        Output visit(GetMineGunAnswerCommand element, params Input[] inputs);
    }

    public class NullType { }
}
