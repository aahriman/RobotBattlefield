﻿using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
    public class DriveAnswerCommandV1_0 : DriveAnswerCommand, ACommand.Sendable {
	    private const string NAME = "DRIVE_ANSWER";
        public static readonly ICommandFactory FACTORY = new DriveAnswerCommandFactory();
        private sealed class DriveAnswerCommandFactory : ACommandFactory {

            public override Boolean IsDeserializable(String s) {
	            string rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    if (rest.Equals("1") || rest.Equals("0")) {
                        cache.Cached(s, new DriveAnswerCommandV1_0(rest.Equals("1")));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is DriveAnswerCommand) {
                    cache.Cached(c, new DriveAnswerCommandV1_0(((DriveAnswerCommand)c).SUCCES));
                    return true;
                }
                return false;
            }
        }

        public DriveAnswerCommandV1_0(bool succes) : base(succes){ }

        public string serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, (SUCCES? 1 : 0));
        }
    }
}
