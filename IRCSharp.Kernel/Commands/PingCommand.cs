﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Manager.Commands
{
	public class PingCommand : ResponseCommandBase
	{
		public override string Execute(IRCSharp.Kernel.Query.IRCCommandQuery query)
		{
			string[] commands = query.Parameter.Split(':');

			return "PONG " + commands[1];
		}

		public override IRCSharp.Kernel.Query.ResponseCommand Name
		{
			get { return IRCSharp.Kernel.Query.ResponseCommand.PING; }
		}
	}
}
