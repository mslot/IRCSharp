using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Manager.Commands
{
	[IRCCommandAttribute(Model.Query.ResponseCommand.PING)]
	public class PingCommand : ResponseCommandBase
	{
		public override Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			string[] commands = query.Parameter.Split(':');
			Model.Query.IRCCommandQuery pongQuery = null;

			Parser.IRC.IRCQueryParser.TryParse("PONG " + commands[1], out pongQuery);

			return pongQuery;
		}

		public override void Init()
		{
		}
	}
}
