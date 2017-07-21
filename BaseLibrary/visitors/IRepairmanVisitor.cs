using BaseLibrary.command.repairman;

namespace BaseLibrary.visitors {
    public interface IRepairmanVisitor {
        void visit(RepairCommand visitor);
        void visit(RepairAnswerCommand visitor);
    }

    public interface IRepairmanVisitor<out Output> {
        Output visit(RepairCommand visitor);
        Output visit(RepairAnswerCommand visitor);
    }
    
    public interface IRepairmanVisitor<out Output, in Input> {
        Output visit(RepairCommand visitor, Input input);
        Output visit(RepairAnswerCommand visitor, Input input);
    }
}