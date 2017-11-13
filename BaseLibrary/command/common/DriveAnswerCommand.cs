﻿using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
    /// <summary>
    /// Answer for drive command.
    /// </summary>
    public class DriveAnswerCommand : ACommonCommand {
        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// True if robot change direction.
        /// </summary>
        public bool SUCCESS { get; private set; }

        public DriveAnswerCommand() {}

        public DriveAnswerCommand(bool success) {
            SUCCESS = success;
            pending = false;
        }

        public void FillData(DriveAnswerCommand source) {
            SUCCESS = source.SUCCESS;
            pending = false;
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
