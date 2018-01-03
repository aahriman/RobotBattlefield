using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BaseLibrary.command;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;

namespace BaseLibrary {
    /// <summary>
    /// Stream for communication by sockets with server.
    /// </summary>
	public class NetworkStream {

		private static int ID = 0;

		private int id = ID++;

		protected readonly System.Net.Sockets.NetworkStream ns;
		protected readonly StreamReader sr;
		protected readonly StreamWriter sw;
		protected AProtocol _protocol = null;
		public AProtocol PROTOCOL {
			get {
				return _protocol;
			}
			set {
				if (_protocol == null || _protocol == value) {
					_protocol = value;
				} else {
					throw new Exception("Protocol can be set only once.");
				}
			}
		}

        /// <summary>
        /// Create instance from socket.
        /// </summary>
        /// <param name="s"></param>
		public NetworkStream(Socket s) {
			ns = new System.Net.Sockets.NetworkStream(s, true);
			sr = new StreamReader(ns, System.Text.Encoding.UTF8);
			sw = new StreamWriter(ns, System.Text.Encoding.UTF8) {
				NewLine = "\n",
				AutoFlush = true
			};
		}

        /// <summary>
        /// Read asynchronously one line from stream.
        /// </summary>
        /// <returns></returns>
		public async Task<string> ReadLineAsync() {
			return await sr.ReadLineAsync();
		}

        /// <summary>
        /// Write asynchronously one line to stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
		public async Task WriteLineAsync(string s) {
		    try {
		        await sw.WriteLineAsync(s);
		    } catch (IOException) {
		        // socket was closed
                this.Close();
		    }
		}

        /// <summary>
        /// Synchronously receive command from stream. That is mean read line and deserialize command by <code>PROTOCOL</code>.
        /// </summary>
        /// <exception cref="ArgumentException">When protocol can not deserialize line.</exception>
        /// <returns></returns>
		public virtual ACommand ReceiveCommand() {
		    return ReceiveCommandAsync().Result;
		}

        /// <summary>
        /// Asynchronously receive command from stream. That is mean read line and deserialize command by <code>PROTOCOL</code>.
        /// </summary>
        /// <exception cref="ArgumentException">When protocol can not deserialize line.</exception>
        /// <returns></returns>
	    public virtual async Task<ACommand> ReceiveCommandAsync() {
	        if (_protocol == null) {
	            throw new Exception("Cannot read or write before set protocol.");
	        }

	        string s = await sr.ReadLineAsync();
	        ACommand command = PROTOCOL.GetCommand(s);
	        if (command == null) {
	            throw new ArgumentException("Protocol (" + _protocol.GetType().Name + ") can not deserialize string - " +
	                                        s);
	        }

	        if (command is ErrorCommand) {
	            Console.Error.WriteLine(((ErrorCommand) command).MESSAGE);
	        }
	        return command;
	    }

        /// <summary>
        /// Synchronously send command.
        /// </summary>
        /// <param name="command"></param>
	    public virtual void SendCommand(ACommand command) {
	        SendCommandAsync(command).Wait();
	    }

        /// <summary>
        /// Asynchronously send command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual async Task SendCommandAsync(ACommand command) {
			if (_protocol == null) {
				throw new Exception("Cannot read or write before set protocol.");
			}

            string serializedCommand = PROTOCOL.GetSendableCommand(command);

            await WriteLineAsync(serializedCommand);
		}

	    private bool close = false;

        /// <summary>
        /// Close stream.
        /// </summary>
		public void Close() {
		    if (!close) {
		        sw.Dispose();
		        sr.Dispose();
		        ns.Close();
		        close = true;
		    }
		}
	}
}
