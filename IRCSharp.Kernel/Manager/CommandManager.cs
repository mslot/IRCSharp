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
		private Collections.SynchronizedDictionary<string, List<ICommand<string, IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery>>> _userdefinedCommands = new Collections.SynchronizedDictionary<string, List<ICommand<string, IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery>>>();
		private Collections.SynchronizedDictionary<Kernel.Parser.IRC.ResponseCommand, List<ICommand<Kernel.Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query>>> _ircCommands = new Collections.SynchronizedDictionary<Kernel.Parser.IRC.ResponseCommand, List<ICommand<Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query>>>();

		public CommandManager()
		{
			this.InsertIRCCommand(new IRCSharp.Kernel.Manager.Commands.PingCommand());
		}

		/// <summary>
		/// Inserts a command. If the command is allready in the collection. The old command is removed, and the new command
		/// is inserted. The command is identified by the name of the command.
		/// </summary>
		/// <param name="command">The command to be inserted.</param>
		public void InsertUserdefinedCommand(ICommand<string, IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery> command)
		{
			if (!_userdefinedCommands.ContainsKey(command.Name))
			{
				_userdefinedCommands[command.Name] = new List<ICommand<string, IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery>>(); //TODO: DANGER. Locks the collection from being written to. Could this be rewritten?
			}

			_userdefinedCommands[command.Name].Add(command);
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
		public List<ICommand<string, IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery>> GetUserdefinedCommand(string name)
		{
			if (_userdefinedCommands.ContainsKey(name))
			{
				return new List<ICommand<string, IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery>>(_userdefinedCommands[name]);
			}
			else
			{
				return null;
			}
		}

		public void InsertIRCCommand(ICommand<Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query> command)
		{
			if (!_ircCommands.ContainsKey(command.Name))
			{
				_ircCommands[command.Name] = new List<ICommand<Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query>>();
			}

			_ircCommands[command.Name].Add(command);
		}

		public List<ICommand<Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query>> GetIRCCommand(Kernel.Parser.IRC.ResponseCommand type)
		{
			if (_ircCommands.ContainsKey(type))
			{
				return new List<ICommand<Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query>>(_ircCommands[type]); //TODO: DANGER. Locks the collection from being written to. Could this be rewritten?
			}
			else
			{
				return null;
			}
		}
	}
}
