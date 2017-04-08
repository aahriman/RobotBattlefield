using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BaseLibrary;
using BaseLibrary.protocol;

namespace ClientLibrary.config {
    public static class ClientConfig {
        public static readonly string[] SUPPERTED_PROTOCOLS;
        public static readonly ProtocolFactory PROTOCOL_FACTORY = new ProtocolFactory();

        
        static ClientConfig() {
            int index = 0;
            KeyValuePair<String, AProtocol>[] supportedProtocols = getSupportedProcotols();
            SUPPERTED_PROTOCOLS = new string[supportedProtocols.Length];
            foreach (var pair in supportedProtocols) {
                SUPPERTED_PROTOCOLS[index++] = pair.Key;
                PROTOCOL_FACTORY.RegisterProtocol(pair.Key, pair.Value);
            }
        }

        private static KeyValuePair<string, AProtocol>[] getSupportedProcotols() {
            List<KeyValuePair<string, AProtocol>> supportedProtocols = new List<KeyValuePair<string, AProtocol>>();
            Type [] protocolTypes = (Type[]) Assembly.GetAssembly(typeof(AProtocol))
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
