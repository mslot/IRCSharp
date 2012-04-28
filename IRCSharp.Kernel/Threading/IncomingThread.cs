using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Threading
{
	public class IncomingThread : IRCSharp.Kernel.Threading.Base.Thread
	{
		private IRCSharp.Kernel.Parser.IRC.Query _query = null;
		private System.IO.TextWriter _textWriter = null;
		private Manager.CommandManager _commandManager = null;

		public IncomingThread(IRCSharp.Kernel.Parser.IRC.Query query, Manager.CommandManager commandManager, System.IO.TextWriter textWriter)
			: base("incoming_thread")
		{
			_commandManager = commandManager;
			_textWriter = textWriter;
			_query = query;
		}

		public override void Task()
		{
			List<IRCSharp.Kernel.ICommand<Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query>> ircCommands = _commandManager.GetIRCCommand(_query.Command);

			if (ircCommands != null)
			{
				foreach (var command in ircCommands)
				{
					string ircCommandOutput = command.Execute(_query);
					(new OutputThread(_textWriter, ircCommandOutput)).Start();
				}
			}
			//lookup userdefined command, and create response thread to respond.
			//TODO parse command parameter to find userdefined command
		}
	}
}
