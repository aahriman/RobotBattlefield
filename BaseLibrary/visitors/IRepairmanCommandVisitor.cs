using BaseLibrary.command.repairman;

namespace BaseLibrary.visitors {
    public interface IRepairmanCommandVisitor {
        void visit(RepairCommand visitor);
        void visit(RepairAnswerCommand visitor);
    }

    public interface IRepairmanCommandVisitor<out Output> {
        Output visit(RepairCommand visitor);
        Output visit(RepairAnswerCommand visitor);
    }
    
    public interface IRepairmanCommandVisitor<out Output, in Input> {
        Output visit(RepairCommand visitor, Input input);
        Output visit(RepairAnswerCommand visitor, Input input);
    }
}