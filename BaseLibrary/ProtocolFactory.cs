﻿using System;
using System.Collections.Generic;
using BaseLibrary.protocol;

namespace BaseLibrary {
    public class ProtocolFactory {

        private readonly IDictionary<string, AProtocol> protocolsMap = new Dictionary<string, AProtocol>();

        public AProtocol GetProtocol(params string[] protocolLabels) {
            string trash;
            return GetProtocol(out trash, protocolLabels);
        }

        public AProtocol GetProtocol(out string selectedProtocolLabel, params string[] protocolLabels) {
            AProtocol protocol;
            foreach (String protocolLabel in protocolLabels) {
                if (protocolsMap.TryGetValue(protocolLabel, out protocol)) {
                    selectedProtocolLabel = protocolLabel;
                    return protocol;
                }
            }
            selectedProtocolLabel = null;
            return null;
        }

        public void RegisterProtocol(string protocolLabel, AProtocol protocol) {
            protocolsMap.Add(protocolLabel, protocol);
        }
    }
}
