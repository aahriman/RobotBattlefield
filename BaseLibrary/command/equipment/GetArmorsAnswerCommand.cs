﻿using System.Collections.Generic;
using BaseLibrary.equip;
using BaseLibrary.visitors;

namespace BaseLibrary.command.equipment {
    public class GetArmorsAnswerCommand : ACommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public static GetArmorsAnswerCommand GetInstance(Armor[] armors) {
            return new GetArmorsAnswerCommand(armors);
        }

        public Armor[] ARMORS { get; private set; }

        public GetArmorsAnswerCommand(Armor[] armors)
            : base() {
            ARMORS = new Armor[armors.Length];
            for (int i = 0; i < ARMORS.Length; i++) {
                ARMORS[i] = armors[i];
            }
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
