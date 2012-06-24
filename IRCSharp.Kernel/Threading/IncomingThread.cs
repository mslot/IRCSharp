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
			List<IRCSharp.Kernel.ICommand<Query.ResponseCommand, Query.IRCCommandQuery>> ircCommands = _commandManager.GetIRCCommand(_query.Command);

			if (ircCommands != null)
			{
				foreach (var command in ircCommands)
				{
					Query.IRCCommandQuery ircCommandOutput = command.Execute(_query);
					(new OutputThread(_ircWriter, ircCommandOutput)).Start(); //TODO: Overkill??
				}
			}

			Query.UserdefinedCommandQuery userdefinedCommandQuery;
			if (Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(_query, out userdefinedCommandQuery))
			{
				List<IRCSharp.Kernel.ICommand<string, Query.UserdefinedCommandQuery>> userdefinedCommands = _commandManager.GetUserdefinedCommand(userdefinedCommandQuery.CommandName);

				if (userdefinedCommands != null)
				{
					foreach (var userdefinedCommand in userdefinedCommands)
					{
						Query.IRCCommandQuery output = userdefinedCommand.Execute(userdefinedCommandQuery);
						(new OutputThread(_ircWriter, output)).Start(); //TODO: Overkill??
					}
				}
			}

		}
	}
}
