﻿using System.Collections.Generic;
using BaseLibrary.equip;
using BaseLibrary.visitors;

namespace BaseLibrary.command.equipment {
    public class GetMineGunAnswerCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public MineGun[] MINE_GUNS { get; private set; }

        public GetMineGunAnswerCommand(MineGun[] mineGuns)
            : base() {
            MINE_GUNS = mineGuns;
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
