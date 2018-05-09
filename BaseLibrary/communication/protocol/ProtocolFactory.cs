using System.Collections.Generic;

namespace BaseLibrary.communication.protocol {
    /// <summary>
    /// Factory for getting implementation of protocol by its name.
    /// </summary>
    public class ProtocolFactory {

        private readonly IDictionary<string, AProtocol> protocolsMap = new Dictionary<string, AProtocol>();

        /// <summary>
        /// Return some protocol with name from protocolLabels.
        /// </summary>
        /// <param name="protocolLabels"></param>
        /// <returns></returns>
        public AProtocol GetProtocol(params string[] protocolLabels) {
            string trash;
            return GetProtocol(out trash, protocolLabels);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedProtocolLabel"></param>
        /// <param name="protocolLabels"></param>
        /// <returns></returns>
        public AProtocol GetProtocol(out string selectedProtocolLabel, params string[] protocolLabels) {
            AProtocol protocol;
            foreach (string protocolLabel in protocolLabels) {
                if (protocolsMap.TryGetValue(protocolLabel, out protocol)) {
                    selectedProtocolLabel = protocolLabel;
                    return protocol;
                }
            }
            selectedProtocolLabel = null;
            return null;
        }

        /// <summary>
        /// Register protocol by its name.
        /// </summary>
        /// <param name="protocolLabel"></param>
        /// <param name="protocol"></param>
        public void RegisterProtocol(string protocolLabel, AProtocol protocol) {
            protocolsMap.Add(protocolLabel, protocol);
        }
    }
}
