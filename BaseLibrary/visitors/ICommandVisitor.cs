using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.equipment;
using BaseLibrary.command.handshake;

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

        Output visit(GetArmorsCommand visitor, Input input);
        Output visit(GetArmorsAnswerCommand visitor, Input input);

        Output visit(GetMotorsCommand visitor, Input input);
        Output visit(GetMotorsAnswerCommand visitor, Input input);

        Output visit(InitCommand visitor, Input input);
        Output visit(InitAnswerCommand visitor, Input input);

        Output visit(DriveCommand visitor, Input input);
        Output visit(DriveAnswerCommand visitor, Input input);

        Output visit(RobotStateCommand visitor, Input input);

        Output visit(ScanCommand visitor, Input input);
        Output visit(ScanAnswerCommand visitor, Input input);

        Output visit(WaitCommand visitor, Input input);

        Output visit(EndLapCommand visitor, Input input);

        Output visit(MerchantCommand visitor, Input input);
        Output visit(MerchantAnswerCommand visitor, Input input);

        Output visit(EndMatchCommand visitor, Input input);

        Output visit(GameTypeCommand visitor, Input input);

        Output visit(GetGunsCommand element, Input input);
        Output visit(GetGunsAnswerCommand element, Input input);

        Output visit(GetRepairToolCommand element, Input input);
        Output visit(GetRepairToolAnswerCommand element, Input input);

        Output visit(GetMineGunCommand element, Input input);
        Output visit(GetMineGunAnswerCommand element, Input input);
    }

    public class NullType { }
}
