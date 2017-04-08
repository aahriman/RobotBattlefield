﻿using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
	internal class EndMatchCommandV1_0 : EndMatchCommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string rest;
				if (StringUtils.GetRestOfString(s, "END_MATCH(", ")", out rest)) {
                    cache.Cached(s, new EndMatchCommandV1_0(rest));
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is EndMatchCommand) {
                    var c2 = (EndMatchCommand)c;
                    cache.Cached(c, new EndMatchCommandV1_0(c2.FILE_URL));
                    return true;
                }
                return false;
            }
        }

        public EndMatchCommandV1_0(String fileUrl) : base(fileUrl) { }


        public string serialize() {
            return String.Format("END_MATCH({0})", FILE_URL);
        }
    }
}
