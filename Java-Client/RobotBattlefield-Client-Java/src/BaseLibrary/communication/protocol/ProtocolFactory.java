package BaseLibrary.communication.protocol;

import java.util.HashMap;

import BaseLibrary.utils.Holder;

public class ProtocolFactory {

	private final HashMap<String, AProtocol> protocolsMap = new HashMap<>();

	public AProtocol GetProtocol(String... protocolLabels) {
		Holder<String> trash = new Holder<>();

		return GetProtocol(trash, protocolLabels);
	}

	public AProtocol GetProtocol(Holder<String> selectedProtocolLabel, String... protocolLabels) {
		AProtocol protocol;
		for (String protocolLabel : protocolLabels) {
			if ((protocol = protocolsMap.get(protocolLabel)) != null) {
				selectedProtocolLabel.value = protocolLabel;
				return protocol;
			}
		}
		return null;
	}

	public void RegisterProtocol(String protocolLabel, AProtocol protocol) {
		protocolsMap.put(protocolLabel, protocol);
	}
	
}
