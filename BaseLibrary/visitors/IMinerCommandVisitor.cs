﻿using BaseLibrary.command.miner;

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
        Output visit(PutMineCommand visitor, Input input);
        Output visit(PutMineAnswerCommand visitor, Input input);

        Output visit(DetonateMineCommand visitor, Input input);
        Output visit(DetonateMineAnswerCommand visitor, Input input);
    }
}
