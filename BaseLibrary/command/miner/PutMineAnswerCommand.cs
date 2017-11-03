using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.miner {
    public class PutMineAnswerCommand : AMinerCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public const int FALSE_MINE_ID = 0;

        public bool SUCCESS { get; private set; }
        public int MINE_ID { get; private set; }

        public PutMineAnswerCommand() { }

        public PutMineAnswerCommand(bool success, int mineId) {
            SUCCESS = success;
            MINE_ID = mineId;
            pending = false;
        }

        public void FillData(PutMineAnswerCommand source) {
            SUCCESS = source.SUCCESS;
            MINE_ID = source.MINE_ID;
            pending = false;
        }

        public override void accept(IMinerVisitor accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(IMinerVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(IMinerVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
