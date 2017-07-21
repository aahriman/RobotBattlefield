using BaseLibrary.command.tank;

namespace BaseLibrary.visitors {
    public interface ITankVisitor {

        void visit(ShotCommand visitor);
        void visit(ShotAnswerCommand visitor);
    }

    public interface ITankVisitor<out Output> {

        Output visit(ShotCommand visitor);
        Output visit(ShotAnswerCommand visitor);
    }

    public interface ITankVisitor<out Output, in Input> {

        Output visit(ShotCommand visitor, Input input);
        Output visit(ShotAnswerCommand visitor, Input input);
    }
}
