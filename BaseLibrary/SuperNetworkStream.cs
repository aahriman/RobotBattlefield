﻿using System;
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
	public class SuperNetworkStream {

		private static int ID = 0;

		private int id = ID++;

		protected readonly NetworkStream ns;
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

		public SuperNetworkStream(Socket s) {
			ns = new NetworkStream(s, true);
			sr = new StreamReader(ns, System.Text.Encoding.UTF8);
			sw = new StreamWriter(ns, System.Text.Encoding.UTF8) {
				NewLine = "\n",
				AutoFlush = true
			};
		}

		public async Task<string> ReadLineAsync() {
			return await sr.ReadLineAsync();
		}

		public async Task WriteLineAsync(string s) {
		    try {
		        await sw.WriteLineAsync(s);
		    } catch (IOException) {
		        // socket was closed
                this.Close();
		    }
		}

		public virtual ACommand RecieveCommand() {
		    return RecieveCommandAsync().Result;
		}

	    public virtual async Task<ACommand> RecieveCommandAsync() {
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

	    public virtual void SendCommand(ACommand command) {
	        SendCommandAsync(command).Wait();
	    }

        public virtual async Task SendCommandAsync(ACommand command) {
			if (_protocol == null) {
				throw new Exception("Cannot read or write before set protocol.");
			}

            string serializedCommand = PROTOCOL.GetSendableCommand(command);

            Console.WriteLine(serializedCommand);
            await WriteLineAsync(serializedCommand);
		}

	    private bool close = false;
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
