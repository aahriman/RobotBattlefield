using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BaseLibrary;
using BaseLibrary.communication.protocol;

namespace ClientLibrary.config {
    /// <summary>
    /// Config for client side.
    /// </summary>
    public static class ClientConfig {
        /// <summary>
        /// Protocol witch is supported by client.
        /// </summary>
        public static readonly string[] SUPPORTED_PROTOCOLS;

        /// <summary>
        /// Protocol factory for making instance protocol by given protocol string.
        /// </summary>
        public static readonly ProtocolFactory PROTOCOL_FACTORY = new ProtocolFactory();
        
        static ClientConfig() {
            int index = 0;
            KeyValuePair<string, AProtocol>[] supportedProtocols = getSupportedProcotols();
            SUPPORTED_PROTOCOLS = new string[supportedProtocols.Length];
            foreach (var pair in supportedProtocols) {
                SUPPORTED_PROTOCOLS[index++] = pair.Key;
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
