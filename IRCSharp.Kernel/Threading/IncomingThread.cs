using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Threading
{
	public class IncomingThread : IRCSharp.Kernel.Threading.Base.Thread
	{
		private IRCSharp.Kernel.Query.IRCCommandQuery _query = null;
		private System.IO.TextWriter _textWriter = null;
		private Manager.CommandManager _commandManager = null;

		public IncomingThread(IRCSharp.Kernel.Query.IRCCommandQuery query, Manager.CommandManager commandManager, System.IO.TextWriter textWriter)
			: base("incoming_thread")
		{
			_commandManager = commandManager;
			_textWriter = textWriter;
			_query = query;
		}

		public override void Task()
		{
			List<IRCSharp.Kernel.ICommand<Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>> ircCommands = _commandManager.GetIRCCommand(_query.Command);

			if (ircCommands != null)
			{
				foreach (var command in ircCommands)
				{
					string ircCommandOutput = command.Execute(_query);
					(new OutputThread(_textWriter, ircCommandOutput)).Start();
				}
			}

			Query.UserdefinedCommandQuery userdefinedCommandQuery;
			if (Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(_query, out userdefinedCommandQuery))
			{
				var userdefinedCommands = _commandManager.GetUserdefinedCommand(userdefinedCommandQuery.CommandName);

				foreach (var userdefinedCommand in userdefinedCommands)
				{
					string output = userdefinedCommand.Execute(userdefinedCommandQuery);
					(new OutputThread(_textWriter, output)).Start();
				}
			}

		}
	}
}
