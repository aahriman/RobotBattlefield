﻿using System;
using BaseLibrary.config;
using BaseLibrary.protocol;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class HelloCommand : ACommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new HelloCommandFactory();
        private sealed class HelloCommandFactory : ACommandFactory {
            internal HelloCommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                if (s.StartsWith("HELLO "+GameProperties.APP_NAME)) {
                    var protocols = s.Substring(("HELLO "+GameProperties.APP_NAME).Length).Split(' ');
                    if (protocols.Length > 0) {
                        cache.Cached(s, new HelloCommand(protocols));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is HelloCommand) {
                    var c1 = (HelloCommand)c;
                    cache.Cached(c, new HelloCommand(c1.SUPPORTED_PROTOCOLS));
                    return true;
                }
                return false;
            }
        }

        public string Serialize() {
            var text = "HELLO " + GameProperties.APP_NAME;
            foreach (var s in SUPPORTED_PROTOCOLS) {
                text += " " + s;
                text.TrimEnd();
            }
            return text;
        }

        public String[] SUPPORTED_PROTOCOLS { get; private set; }

        public HelloCommand(String[] supportedProtocols)
            : base() {
            Array.Sort(supportedProtocols);
            SUPPORTED_PROTOCOLS = supportedProtocols;
        }

        public override void accept(ICommandVisitor accepter) {
            throw new NotImplementedException();
        }

        public override Output accept<Output>(ICommandVisitor<Output> accepter) {
            throw new NotImplementedException();
        }

        public override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            throw new NotImplementedException();
        }
    }
}
