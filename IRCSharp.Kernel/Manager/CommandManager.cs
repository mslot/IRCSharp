using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Manager
{
	/// <summary>
	/// Handles commands for the system. 
	/// 
	/// TODO: refactor definition of _userdefinedCommands and _ircCommands. Maybe put in seperate class.
	/// </summary>
	public class CommandManager
	{
		private Collections.SynchronizedDictionary<string, List<CommandInformation<string>>> _userdefinedCommands = new Collections.SynchronizedDictionary<string, List<CommandInformation<string>>>(); //TODO refactor this to strong types.
		private Collections.SynchronizedDictionary<Kernel.Query.ResponseCommand, List<CommandInformation<Query.ResponseCommand>>> _ircCommands = new Collections.SynchronizedDictionary<Kernel.Query.ResponseCommand, List<CommandInformation<Query.ResponseCommand>>>(); //TODO refactor this to strong types
		private Collections.SynchronizedDictionary<Kernel.Query.ResponseCommand, List<CommandInformation<Query.ResponseCommand>>> _ircCommandsGlobal = new Collections.SynchronizedDictionary<Kernel.Query.ResponseCommand, List<CommandInformation<Query.ResponseCommand>>>(); //TODO refactor this to strong types

		public CommandManager()
		{
			CommandInformation<Query.ResponseCommand> information = new CommandInformation<Query.ResponseCommand>(
				typeof(IRCSharp.Kernel.Manager.Commands.PingCommand)
				, Query.ResponseCommand.PING
				, String.Empty); //TODO: this can be done prettier.

			InsertIRCCommand(information, information.Name);
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
		public List<CommandInformation<string>> GetUserdefinedCommand(string name)
		{
			return _userdefinedCommands.TryGet(name);
		}

		public List<CommandInformation<Query.ResponseCommand>> GetIRCCommand(Kernel.Query.ResponseCommand type)
		{
			var allGlobalCommands = _ircCommandsGlobal.GetUnsynchronizedDictionary();
			var specificCommands = _ircCommands.TryGet(type);

			var all = new List<CommandInformation<Query.ResponseCommand>>();

			if (specificCommands!= null && specificCommands.Count > 0)
			{
				all.AddRange(specificCommands);
			}

			foreach (var kvp in allGlobalCommands)
			{
				all.AddRange(kvp.Value);
			}

			return all;
		}

		public void InsertIRCCommand(CommandInformation<Query.ResponseCommand> command, Query.ResponseCommand name)
		{
			_ircCommands.TryInsert(name, new List<CommandInformation<Query.ResponseCommand>>());
			_ircCommands[name].Add(command);

			if (name == Query.ResponseCommand.ALL)
			{
				_ircCommandsGlobal.TryInsert(name, new List<CommandInformation<Query.ResponseCommand>>());
				_ircCommandsGlobal[name].Add(command);
			}
		}

		/// <summary>
		/// Inserts a command. If the command is allready in the collection. The old command is removed, and the new command
		/// is inserted. The command is identified by the name of the command.
		/// </summary>
		/// <param name="command">The command to be inserted.</param>
		public void InsertUserdefinedCommand(CommandInformation<string> command, string name)
		{
			_userdefinedCommands.TryInsert(name, new List<CommandInformation<string>>());
			_userdefinedCommands[name].Add(command);
		}
	}
}
