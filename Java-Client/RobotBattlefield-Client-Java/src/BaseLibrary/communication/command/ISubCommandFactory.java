package BaseLibrary.communication.command;

import BaseLibrary.utils.Holder;

/**
 * Serialize and deserialize subcommand (stored in <code>MORE</code>)
 * 
 * @see ACommand.MORE
 */
public interface ISubCommandFactory {

	/**
	 * Deserialize String and paste it like object to commandsMore
	 */
	boolean deserialize(String s, Object[] commandsMore);

	/**
	 * Serialize object
	 */
	boolean serialize(Object singleMore, Holder<String> serializedSingleMore);
	
	boolean insert(Object singleMore, Object[] commandsMore);
}
