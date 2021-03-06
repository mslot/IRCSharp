﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Threading
{
	public class IncomingThread : IRCSharp.Threading.Base.Thread
	{
		private IRCSharp.Kernel.Model.Query.IRCCommandQuery _query = null;
		private Model.Query.Writer.IRCWriter<System.IO.Stream> _ircWriter = null;
		private Manager.CommandManager _commandManager = null;

		public IncomingThread(IRCSharp.Kernel.Model.Query.IRCCommandQuery query, Manager.CommandManager commandManager, Model.Query.Writer.IRCWriter<System.IO.Stream> ircWriter)
			: base("incoming_thread")
		{
			_commandManager = commandManager;
			_ircWriter = ircWriter;
			_query = query;
		}

		public override void Task()
		{
			var ircResults = _commandManager.FireIRCCommand(_query);

			foreach (var result in ircResults)
			{
				(new OutputThread(_ircWriter, result)).Start();
			}

			var userdefinedResults = _commandManager.FireUserdefinedCommand(_query);

			if (userdefinedResults != null)
			{
				foreach (var results in userdefinedResults)
				{
					(new OutputThread(_ircWriter, results)).Start();
				}
			}
		}
	}
}
