﻿namespace BaseLibrary.communication.command {

    /// <summary>
    /// Serialize and deserialize subcommand (stored in <code>MORE</code>)
    /// </summary>
    /// <seealso cref="ACommand.MORE"/>
    public interface ISubCommandFactory {

        /// <summary>
        /// Deserialize string and paste it like object to commandsMore
        /// </summary>
        /// <param name="s"></param>
        /// <param name="commandsMore"></param>
        /// <returns></returns>
        bool Deserialize(string s, object[] commandsMore);

        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="singleMore"></param>
        /// <param name="serializedSingleMore"></param>
        /// <returns></returns>
        bool Serialize(object singleMore, out string serializedSingleMore);
    }
}
