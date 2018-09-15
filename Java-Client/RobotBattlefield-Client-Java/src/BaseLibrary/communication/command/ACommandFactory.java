package BaseLibrary.communication.command;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.IFactory.AFactory;

/**
    * Factory for command
    */
    public abstract class ACommandFactory extends AFactory<ACommand, ACommand> {
        /**
        * Try to Transferable and if it is possible store serialization of transfer object like String.
        */
    	@Override
    	public boolean isSerializeable(ACommand c, AProtocol protocol) {
            if (isTransferable(c)) {
                cacheForSerialize.cached(c, transfer(c).serialize(protocol));
                return true;
            }
            return false;
        }
}
