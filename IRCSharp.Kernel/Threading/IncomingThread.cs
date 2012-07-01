using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Threading
{
	public class IncomingThread : IRCSharp.Threading.Base.Thread
	{
		private IRCSharp.Kernel.Query.IRCCommandQuery _query = null;
		private Query.Writer.IRCWriter<System.IO.Stream> _ircWriter = null;
		private Manager.CommandManager _commandManager = null;

		public IncomingThread(IRCSharp.Kernel.Query.IRCCommandQuery query, Manager.CommandManager commandManager, Query.Writer.IRCWriter<System.IO.Stream> ircWriter)
			: base("incoming_thread")
		{
			_commandManager = commandManager;
			_ircWriter = ircWriter;
			_query = query;
		}

		//TODO: refactor this method and put logic of method out in seperate class.
		public override void Task()
		{
			List<CommandInformation<Query.ResponseCommand>> ircCommandInformation = _commandManager.GetIRCCommand(_query.Command);

			if (ircCommandInformation != null)
			{
				foreach (var commandInformation in ircCommandInformation)
				{
					ICommand<Query.ResponseCommand, Query.IRCCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<Query.ResponseCommand, Query.IRCCommandQuery>>(commandInformation.CommandType);
					if (command != null)
					{
						var query = command.Execute(_query);
						(new OutputThread(_ircWriter, query)).Start(); //TODO: Overkill??
					}
				}
			}

			Query.UserdefinedCommandQuery userdefinedCommandQuery;
			if (Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(_query, out userdefinedCommandQuery))
			{
				List<CommandInformation<string>> userdefinedInformationCommands = _commandManager.GetUserdefinedCommand(userdefinedCommandQuery.CommandName);

				if (userdefinedInformationCommands != null)
				{
					foreach (var userdefinedCommandInformation in userdefinedInformationCommands)
					{
						ICommand<string, Query.UserdefinedCommandQuery> command = Reflection.ReflectionUtil.LoadTypeOf<ICommand<string, Query.UserdefinedCommandQuery>>(userdefinedCommandInformation.CommandType);
						Query.IRCCommandQuery output = command.Execute(userdefinedCommandQuery);
						(new OutputThread(_ircWriter, output)).Start(); //TODO: Overkill??
					}
				}
			}

		}
	}
}
