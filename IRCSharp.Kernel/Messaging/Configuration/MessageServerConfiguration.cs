﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Messaging.Configuration
{
	//TODO move out into some real configuration
	public static class MessageServerConfiguration
	{
		public static string BotServerQueuePath { get { return @".\private$\BotServer"; } }
		public static string BotServerOutgoingPath { get { return @".\private$\BotOutgoing"; } }
	}
}
