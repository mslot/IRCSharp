﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Manager.Commands
{
	public class PingCommand : ResponseCommandBase
	{
		public override Query.IRCCommandQuery Execute(IRCSharp.Kernel.Query.IRCCommandQuery query)
		{
			string[] commands = query.Parameter.Split(':');
			Query.IRCCommandQuery pongQuery = null;

			Parser.IRC.IRCQueryParser.TryParse("PONG " + commands[1], out pongQuery);

			return pongQuery;
		}

		public override IRCSharp.Kernel.Query.ResponseCommand Name
		{
			get { return IRCSharp.Kernel.Query.ResponseCommand.PING; }
		}
	}
}
