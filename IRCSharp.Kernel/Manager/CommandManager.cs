using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Manager
{
	/// <summary>
	/// Handles commands for the system. 
	/// </summary>
	public class CommandManager
	{
		private Collections.SynchronizedDictionary<string, List<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>> _userdefinedCommands = new Collections.SynchronizedDictionary<string, List<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>>>();
		private Collections.SynchronizedDictionary<Kernel.Query.ResponseCommand, List<ICommand<Kernel.Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>>> _ircCommands = new Collections.SynchronizedDictionary<Kernel.Query.ResponseCommand, List<ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>>>();

		public CommandManager()
		{
			this.InsertIRCCommand(new IRCSharp.Kernel.Manager.Commands.PingCommand());
		}

		/// <summary>
		/// Removes the command from the internal collection.
		/// </summary>
		/// <param name="name">Name of command to be removed.</param>
		public void RemoveUserdefinedCommand(string name)
		{
			_userdefinedCommands.Remove(name);
		}

		/// <summary>
		/// returns the command with the given name. Behaviour is undefined if the command does not exists.
		/// </summary>
		/// <param name="name">Name of command to return.</param>
		/// <returns>An object that implements ICommand interface.</returns>
		public List<ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery>> GetUserdefinedCommand(string name)
		{
			return _userdefinedCommands.TryGet(name);
		}

		public List<ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>> GetIRCCommand(Kernel.Query.ResponseCommand type)
		{
			return _ircCommands.TryGet(type);
		}

		public void InsertIRCCommand(ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery> command)
		{
			_ircCommands.TryInsert(command.Name, new List<ICommand<Query.ResponseCommand,Query.IRCCommandQuery>>());
			_ircCommands[command.Name].Add(command);
		}

		/// <summary>
		/// Inserts a command. If the command is allready in the collection. The old command is removed, and the new command
		/// is inserted. The command is identified by the name of the command.
		/// </summary>
		/// <param name="command">The command to be inserted.</param>
		public void InsertUserdefinedCommand(ICommand<string, IRCSharp.Kernel.Query.UserdefinedCommandQuery> command)
		{
			_userdefinedCommands.TryInsert(command.Name, new List<ICommand<string, Query.UserdefinedCommandQuery>>());
			_userdefinedCommands[command.Name].Add(command);
		}
	}
}
