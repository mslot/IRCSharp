using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRCSharp.Security;

namespace IRCSharp.Security
{
	class SecureManager : IRCSharp.Kernel.Manager.ICommandManager
	{
		private IRCSharp.Kernel.Manager.ICommandManager _commandManager;
		private IAuthenticationProvider _authenticationProvider;

		public SecureManager(IRCSharp.Kernel.Manager.ICommandManager commandManager, IAuthenticationProvider authenticationProvider)
		{
			_commandManager = commandManager;
			_authenticationProvider = authenticationProvider;
		}

		public List<IRCSharp.Kernel.Model.Query.IRCCommandQuery> FireUserdefinedCommand(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			List<IRCSharp.Kernel.Model.Query.IRCCommandQuery> commands = null;
			IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery userdefinedCommandQuery;
			if (IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(query, out userdefinedCommandQuery))
			{
				if (_authenticationProvider.AuthenticateUser(query.From))
				{
					if (_authenticationProvider.MayUserFireCommand(query.From, userdefinedCommandQuery.CommandName))
					{
						commands = _commandManager.FireUserdefinedCommand(query); //TODO make a FireUserdefinedCommand(Model.Query.UserdefinedCommandQuery query) in CommandManager. We do not need to parse userdefined query twice.
					}
				}
			}

			return commands;
		}

		public List<IRCSharp.Kernel.Model.Query.IRCCommandQuery> FireIRCCommand(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			return _commandManager.FireIRCCommand(query);
		}

		public void RemoveUserdefinedCommand(string name)
		{
			_commandManager.RemoveUserdefinedCommand(name);
		}

		public List<IRCSharp.Kernel.CommandInformation<string>> GetUserdefinedCommand(string name)
		{
			return _commandManager.GetUserdefinedCommand(name);
		}

		public List<IRCSharp.Kernel.CommandInformation<IRCSharp.Kernel.Model.Query.IRCCommand>> GetIRCCommand(IRCSharp.Kernel.Model.Query.IRCCommand type)
		{
			return _commandManager.GetIRCCommand(type);
		}

		public void InsertIRCCommand(IRCSharp.Kernel.CommandInformation<IRCSharp.Kernel.Model.Query.IRCCommand> command)
		{
			_commandManager.InsertIRCCommand(command);
		}

		public void InsertIRCCommand(Type commandType, IRCSharp.Kernel.Model.Query.IRCCommand name, string absoluteFilePath)
		{
			_commandManager.InsertIRCCommand(commandType, name, absoluteFilePath);
		}

		public void InsertUserdefinedCommand(IRCSharp.Kernel.CommandInformation<string> command)
		{
			_commandManager.InsertUserdefinedCommand(command);
		}

		public void InsertUserdefinedCommand(Type commandType, string name, string absoluteFilePath)
		{
			_commandManager.InsertUserdefinedCommand(commandType, name, absoluteFilePath);
		}

		public void InitAllCommands()
		{
			_commandManager.InitAllCommands();
		}
	}
}
