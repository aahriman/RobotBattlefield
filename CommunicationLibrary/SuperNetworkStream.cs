using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommunicationLibrary.command;
using CommunicationLibrary.protocol;

namespace CommunicationLibrary {
	public class SuperNetworkStream {

		class Item {
			public SuperNetworkStream SNS { get; private set; }
			public String MESSAGE { get; private set; }
			public Task TASK { get; private set; }

			public Item(SuperNetworkStream SNS, String MESSAGE) {
				this.MESSAGE = MESSAGE;
				this.SNS = SNS;
				this.TASK = new Task(() =>
					SNS.sw.WriteLine(MESSAGE)
				);
			}
		}

		private readonly BlockingCollection<Item> queue = new BlockingCollection<Item>();

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
				if (_protocol == null) {
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
			consumentLoop();
		}

		private void consumentLoop() {
			Thread t = new Thread(() => {
				while (true) {
					Item item = queue.Take();
					item.TASK.RunSynchronously();
				}
			}) { IsBackground = true };
			t.Start();
		}

		public async Task<string> ReadLineAsync() {
			return await sr.ReadLineAsync();
		}

		public Task WriteLineAsync(string s) {
			Item i = new Item(this, s);
			queue.Add(i);
			return i.TASK;
		}

		public virtual ACommand RecieveCommand() {
			if (_protocol == null) {
				throw new Exception("Cannot read or write before set protocol.");
			}
			String s = sr.ReadLine();
			ACommand command = PROTOCOL.GetCommand(s);
			if (command == null) {
				throw new ArgumentException("Protocol (" + _protocol.GetType().Name + ") can not deserialize string - " + s);
			}
			return command;
		}

		public virtual async Task<ACommand> RecieveCommandAsync() {
			if (_protocol == null) {
				throw new Exception("Cannot read or write before set protocol.");
			}
			String s = await sr.ReadLineAsync();
			ACommand command = PROTOCOL.GetCommand(s);
			if (command == null) {
				throw new ArgumentException("Protocol (" + _protocol.GetType().Name + ") can not deserialize string - " + s);
			}
			return command;
		}

		public virtual Task SendCommandAsync(ACommand command) {
			if (_protocol == null) {
				throw new Exception("Cannot read or write before set protocol.");
			}
			ACommand.Sendable commandSendable = PROTOCOL.GetSendableCommand((ACommand) command);
			if (commandSendable == null) {
				throw new ArgumentException("Protocol (" + _protocol.GetType().Name + ") do not know command type: " + command.GetType().Name);
			}
			return commandSendable.SendAsync(this);
		}

		public virtual async void SendCommandAsyncDontWait(ACommand command) {
			await Task.Yield();
			await SendCommandAsync(command);
		}

		public void Close() {
			sw.Dispose();
			sr.Dispose();
			ns.Close();
		}
	}
}
