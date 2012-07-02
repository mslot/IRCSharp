using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Manager
{
	/// <summary>
	/// Handles commands for the system. 
	/// 
	/// TODO: More method comments. Some is deprecated!!
	/// </summary>
	public class CommandManager
	{
		private Collections.UserdefinedCommandCollection _userdefinedCommands = new Collections.UserdefinedCommandCollection();
		private Collections.IRCCommandCollection _ircCommands = new Collections.IRCCommandCollection();
		private Collections.IRCCommandCollection _ircCommandsGlobal = new Collections.IRCCommandCollection();

		public CommandManager()
		{
			CommandInformation<Query.ResponseCommand> information = new CommandInformation<Query.ResponseCommand>(
				typeof(IRCSharp.Kernel.Manager.Commands.PingCommand)
				, Query.ResponseCommand.PING
				, String.Empty); //TODO: this can be done prettier.

			InsertIRCCommand(information);
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

			if (specificCommands != null && specificCommands.Count > 0)
			{
				all.AddRange(specificCommands);
			}

			foreach (var kvp in allGlobalCommands)
			{
				all.AddRange(kvp.Value);
			}

			return all;
		}

		public void InsertIRCCommand(CommandInformation<Query.ResponseCommand> command)
		{
			_ircCommands.TryInsert(command.Name, new List<CommandInformation<Query.ResponseCommand>>());
			_ircCommands[command.Name].Add(command);

			if (command.Name == Query.ResponseCommand.ALL)
			{
				_ircCommandsGlobal.TryInsert(command.Name, new List<CommandInformation<Query.ResponseCommand>>());
				_ircCommandsGlobal[command.Name].Add(command);
			}
		}

		internal void InsertIRCCommand(Type commandType, Query.ResponseCommand name, string absoluteFilePath)
		{
			CommandInformation<Query.ResponseCommand> information = new CommandInformation<Query.ResponseCommand>(commandType, name, absoluteFilePath);
			InsertIRCCommand(information);
		}

		/// <summary>
		/// Inserts a command. If the command is allready in the collection. The old command is removed, and the new command
		/// is inserted. The command is identified by the name of the command.
		/// </summary>
		/// <param name="command">The command to be inserted.</param>
		public void InsertUserdefinedCommand(CommandInformation<string> command)
		{
			_userdefinedCommands.TryInsert(command.Name, new List<CommandInformation<string>>());
			_userdefinedCommands[command.Name].Add(command);
		}

		public void InsertUserdefinedCommand(Type commandType, string name, string absoluteFilePath)
		{
			CommandInformation<string> information = new CommandInformation<string>(commandType, name, absoluteFilePath);
			InsertUserdefinedCommand(information);
		}

		public List<Query.IRCCommandQuery> FireUserdefinedCommand(IRCSharp.Kernel.Query.IRCCommandQuery query)
		{
			List<Query.IRCCommandQuery> results = null;
			Query.UserdefinedCommandQuery userdefinedCommandQuery;
			if (Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(query, out userdefinedCommandQuery))
			{
				List<CommandInformation<string>> userdefinedInformationCommands = this.GetUserdefinedCommand(userdefinedCommandQuery.CommandName);

				if (userdefinedInformationCommands != null)
				{
					results = new List<Query.IRCCommandQuery>();
					foreach (var userdefinedCommandInformation in userdefinedInformationCommands)
					{
						ICommand<string, Query.UserdefinedCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<string, Query.UserdefinedCommandQuery>>(userdefinedCommandInformation.CommandType);
						Query.IRCCommandQuery output = command.Execute(userdefinedCommandQuery);
						results.Add(output);
					}
				}
			}

			return results;
		}

		public List<Query.IRCCommandQuery> FireIRCCommand(IRCSharp.Kernel.Query.IRCCommandQuery query)
		{
			List<CommandInformation<Query.ResponseCommand>> ircCommandInformation = this.GetIRCCommand(query.Command);
			List<Query.IRCCommandQuery> results = null;

			if (ircCommandInformation != null)
			{
				results = new List<Query.IRCCommandQuery>();
				foreach (var commandInformation in ircCommandInformation)
				{
					ICommand<Query.ResponseCommand, Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Query.ResponseCommand, Query.IRCCommandQuery>>(commandInformation.CommandType);
					if (command != null)
					{
						var queryResult = command.Execute(query);
						results.Add(queryResult);
					}
				}
			}

			return results;
		}

		public void InitAllCommands()
		{
			InitUserdefinedCommands();
			InitIRCCommands();
			InitGlobalCommands();
		}

		private void InitGlobalCommands()
		{
			Dictionary<Query.ResponseCommand, List<CommandInformation<Query.ResponseCommand>>> commandInformations = _ircCommandsGlobal.GetUnsynchronizedDictionary();

			foreach (var kvpCommands in commandInformations)
			{
				foreach (var commandInformation in kvpCommands.Value)
				{
					ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>>(commandInformation.CommandType);
					command.Init();
				}
			}
		}

		private void InitIRCCommands()
		{
			Dictionary<Query.ResponseCommand, List<CommandInformation<Query.ResponseCommand>>> commandInformations = _ircCommands.GetUnsynchronizedDictionary();

			foreach (var kvpCommands in commandInformations)
			{
				foreach (var commandInformation in kvpCommands.Value)
				{
					ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>>(commandInformation.CommandType);
					command.Init();
				}
			}
		}

		private void InitUserdefinedCommands()
		{
			Dictionary<string, List<CommandInformation<string>>> commandInformations = _userdefinedCommands.GetUnsynchronizedDictionary();

			foreach (var kvpCommands in commandInformations)
			{
				foreach (var commandInformation in kvpCommands.Value)
				{
					ICommand<string, IRCSharp.Kernel.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<string, IRCSharp.Kernel.Query.IRCCommandQuery>>(commandInformation.CommandType);
					command.Init();
				}
			}
		}
	}
}
