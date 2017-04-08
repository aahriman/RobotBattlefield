using BaseLibrary.command.miner;

namespace BaseLibrary.visitors {
    public interface IMinerCommandVisitor {
        void visit(PutMineCommand visitor);
        void visit(PutMineAnswerCommand visitor);

        void visit(DetonateMineCommand visitor);
        void visit(DetonateMineAnswerCommand visitor);
    }

    public interface IMinerCommandVisitor<out Output> {
        Output visit(PutMineCommand visitor);
        Output visit(PutMineAnswerCommand visitor);

        Output visit(DetonateMineCommand visitor);
        Output visit(DetonateMineAnswerCommand visitor);
    }

    public interface IMinerCommandVisitor<out Output, in Input> {
        Output visit(PutMineCommand visitor, params Input[] inputs);
        Output visit(PutMineAnswerCommand visitor, params Input[] inputs);

        Output visit(DetonateMineCommand visitor, params Input[] inputs);
        Output visit(DetonateMineAnswerCommand visitor, params Input[] inputs);
    }
}
