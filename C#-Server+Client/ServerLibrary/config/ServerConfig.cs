using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BaseLibrary.communication.protocol;
using BaseLibrary.equipment;

namespace ServerLibrary.config {

    public static class ServerConfig {
        public static string[] SUPPORTED_PROTOCOLS { get; private set; }
        public static readonly ProtocolFactory PROTOCOL_FACTORY = new ProtocolFactory();

        public static readonly int MAX_TURN = 5000;

        static ServerConfig() {
            int index = 0;

            KeyValuePair<string, AProtocol>[] supportedProtocols = getSupportedProtocols();
            SUPPORTED_PROTOCOLS = new string[supportedProtocols.Length];
            foreach (var pair in supportedProtocols) {
                SUPPORTED_PROTOCOLS[index++] = pair.Key;
                PROTOCOL_FACTORY.RegisterProtocol(pair.Key, pair.Value);
            }           
        }

        private static KeyValuePair<string, AProtocol>[] getSupportedProtocols() {
            List<KeyValuePair<string, AProtocol>> supportedProtocols = new List<KeyValuePair<string, AProtocol>>();
            Type[] protocolTypes = (Type[]) Assembly.GetAssembly(typeof(AProtocol))
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    typeof(AProtocol).IsAssignableFrom(t) &&
                    t.GetCustomAttribute(typeof(ProtocolDescription)) != null)
                .ToArray();
            foreach (Type protocolType in protocolTypes) {
                AProtocol protocol = (AProtocol) Activator.CreateInstance(protocolType);
                string name =
                    ((ProtocolDescription) protocolType.GetCustomAttribute(typeof(ProtocolDescription))).NAME;
                KeyValuePair<string, AProtocol> keyValuePair = new KeyValuePair<string, AProtocol>(name, protocol);
                supportedProtocols.Add(keyValuePair);
            }
            return supportedProtocols.ToArray();
        }

    }
}
