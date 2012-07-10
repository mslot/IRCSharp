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
			InsertIRCCommand(typeof(IRCSharp.Kernel.Manager.Commands.PingCommand),
				Model.Query.ResponseCommand.PING, 
				String.Empty);
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

		public List<CommandInformation<Model.Query.ResponseCommand>> GetIRCCommand(Kernel.Model.Query.ResponseCommand type)
		{
			var allGlobalCommands = _ircCommandsGlobal.GetUnsynchronizedDictionary();
			var specificCommands = _ircCommands.TryGet(type);

			var all = new List<CommandInformation<Model.Query.ResponseCommand>>();

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

		public void InsertIRCCommand(CommandInformation<Model.Query.ResponseCommand> command)
		{
			_ircCommands.TryInsert(command.Name, new List<CommandInformation<Model.Query.ResponseCommand>>());
			_ircCommands[command.Name].Add(command);

			if (command.Name == Model.Query.ResponseCommand.ALL)
			{
				_ircCommandsGlobal.TryInsert(command.Name, new List<CommandInformation<Model.Query.ResponseCommand>>());
				_ircCommandsGlobal[command.Name].Add(command);
			}

			//TODO: remember to init the command
		}

		internal void InsertIRCCommand(Type commandType, Model.Query.ResponseCommand name, string absoluteFilePath)
		{
			CommandInformation<Model.Query.ResponseCommand> information = new CommandInformation<Model.Query.ResponseCommand>(commandType, name, absoluteFilePath);
			InsertIRCCommand(information);

			//TODO: remember to init the command
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

			//TODO: remember to init the command
		}

		public void InsertUserdefinedCommand(Type commandType, string name, string absoluteFilePath)
		{
			CommandInformation<string> information = new CommandInformation<string>(commandType, name, absoluteFilePath);
			InsertUserdefinedCommand(information);

			//TODO: remember to init the command
		}

		public List<Model.Query.IRCCommandQuery> FireUserdefinedCommand(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			List<Model.Query.IRCCommandQuery> results = null;
			Model.Query.UserdefinedCommandQuery userdefinedCommandQuery;
			if (Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(query, out userdefinedCommandQuery))
			{
				List<CommandInformation<string>> userdefinedInformationCommands = this.GetUserdefinedCommand(userdefinedCommandQuery.CommandName);

				if (userdefinedInformationCommands != null)
				{
					results = new List<Model.Query.IRCCommandQuery>();
					foreach (var userdefinedCommandInformation in userdefinedInformationCommands)
					{
						ICommand<string, Model.Query.UserdefinedCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<string, Model.Query.UserdefinedCommandQuery>>(userdefinedCommandInformation.CommandType);
						Model.Query.IRCCommandQuery output = command.Execute(userdefinedCommandQuery);

						if (output != null)
						{
							results.Add(output);
						}
					}
				}
			}

			return results;
		}

		public List<Model.Query.IRCCommandQuery> FireIRCCommand(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			List<CommandInformation<Model.Query.ResponseCommand>> ircCommandInformation = this.GetIRCCommand(query.Command);
			List<Model.Query.IRCCommandQuery> results = null;

			if (ircCommandInformation != null)
			{
				results = new List<Model.Query.IRCCommandQuery>();
				foreach (var commandInformation in ircCommandInformation)
				{
					ICommand<Model.Query.ResponseCommand, Model.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Model.Query.ResponseCommand, Model.Query.IRCCommandQuery>>(commandInformation.CommandType);
					if (command != null)
					{
						var queryResult = command.Execute(query);

						if (queryResult != null)
						{
							results.Add(queryResult);
						}
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
			Dictionary<Model.Query.ResponseCommand, List<CommandInformation<Model.Query.ResponseCommand>>> commandInformations = _ircCommandsGlobal.GetUnsynchronizedDictionary();

			foreach (var kvpCommands in commandInformations)
			{
				foreach (var commandInformation in kvpCommands.Value)
				{
					ICommand<Model.Query.ResponseCommand, IRCSharp.Kernel.Model.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Model.Query.ResponseCommand, IRCSharp.Kernel.Model.Query.IRCCommandQuery>>(commandInformation.CommandType);
					command.Init();
				}
			}
		}

		private void InitIRCCommands()
		{
			Dictionary<Model.Query.ResponseCommand, List<CommandInformation<Model.Query.ResponseCommand>>> commandInformations = _ircCommands.GetUnsynchronizedDictionary();

			foreach (var kvpCommands in commandInformations)
			{
				foreach (var commandInformation in kvpCommands.Value)
				{
					ICommand<Model.Query.ResponseCommand, IRCSharp.Kernel.Model.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Model.Query.ResponseCommand, IRCSharp.Kernel.Model.Query.IRCCommandQuery>>(commandInformation.CommandType);
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
					ICommand<string, IRCSharp.Kernel.Model.Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<string, IRCSharp.Kernel.Model.Query.IRCCommandQuery>>(commandInformation.CommandType);
					command.Init();
				}
			}
		}
	}
}
