﻿using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.repairman {
    public class RepairAnswerCommand : ARepairmanCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public bool SUCCESS { get; private set; }

        public RepairAnswerCommand(bool succes) {
            SUCCESS = succes;
        }

        public override void accept(IRepairmanVisitor accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(IRepairmanVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(IRepairmanVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
