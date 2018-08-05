using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BaseLibrary.communication.command {
    /// <summary>
    /// Processing commands
    /// </summary>
    public class CommandProcessor<TOutput, TInput> {

        public delegate TOutput DefaultProcess([NotNull] ACommand command);
        public delegate TOutput SingleProcess<in TCommand>([NotNull] TCommand command, TInput input) where TCommand : ACommand;

        protected IDictionary<Type, SingleProcess<ACommand>> typeProcesses = new Dictionary<Type, SingleProcess<ACommand>>();

        /// <summary>
        /// Process witch will be done when no process was found.
        /// </summary>
        [NotNull]
        protected DefaultProcess defaultProcess;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultProcess">Process witch will be done when no process was found.</param>
        public CommandProcessor([NotNull] DefaultProcess defaultProcess) {
            this.defaultProcess = defaultProcess ?? throw new ArgumentNullException(nameof(defaultProcess));
        }

        /// <summary>
        /// Register process for command.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="process"></param>
        public void RegisterProcess<TCommand>([NotNull] SingleProcess<TCommand> process) where TCommand : ACommand{
            if (process == null) {
                throw new ArgumentNullException(nameof(process));
            }
            typeProcesses.Add(typeof(TCommand), (command, input) => process((TCommand) command, input));
        }

        /// <summary>
        /// Remove registered process for type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns><code>true</code> -- if type is successfully removed</returns>
        /// <seealso cref="Dictionary{TKey,TValue}.Remove"/>
        public bool RemoveRegistration(Type type) {
            return typeProcesses.Remove(type);
        }

        /// <summary>
        /// Process command with input.
        /// 
        /// Find registered process for command type and process command by them.
        /// </summary>
        /// <param name="command">Processed command.</param>
        /// <param name="input"></param>
        /// <exception cref="ArgumentNullException">when command is null</exception>
        /// <returns></returns>
        public TOutput Process([NotNull] ACommand command, TInput input) {
            if (command == null) {
                throw new ArgumentNullException(nameof(command));
            }
            Type type = command.GetType();
            
            do {
                if (typeProcesses.TryGetValue(type, out SingleProcess<ACommand> process)) {
                    return process(command, input);
                }
                type = type.BaseType;
            } while (type != null);
            
            return defaultProcess(command);
        }

        /// <summary>
        /// Get registered process for type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public bool TryGetProcess(Type type, out SingleProcess<ACommand> process) {
            return typeProcesses.TryGetValue(type, out process);
        }
    }
}
