using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class ScanAnswerCommand : ACommand{

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public static ScanAnswerCommand GetInstance(ProtocolDouble range, int enemyID) {
            return new ScanAnswerCommand(range, enemyID);
        }

        public ProtocolDouble RANGE {get; private set;}
        public int ENEMY_ID { get; private set; }

        public ScanAnswerCommand(ProtocolDouble range, int enemyID)
            : base() {
                ENEMY_ID = enemyID;
                RANGE = range;
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
