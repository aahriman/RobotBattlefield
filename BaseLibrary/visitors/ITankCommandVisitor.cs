using BaseLibrary.command.tank;

namespace BaseLibrary.visitors {
    public interface ITankCommandVisitor {

        void visit(ShotCommand visitor);
        void visit(ShotAnswerCommand visitor);
    }

    public interface ITankCommandVisitor<out Output> {

        Output visit(ShotCommand visitor);
        Output visit(ShotAnswerCommand visitor);
    }

    public interface ITankCommandVisitor<out Output, in Input> {

        Output visit(ShotCommand visitor, params Input[] inputs);
        Output visit(ShotAnswerCommand visitor, params Input[] inputs);
    }
}
