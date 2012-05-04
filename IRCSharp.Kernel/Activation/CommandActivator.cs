using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Activation
{
	public class CommandActivator : ICommandActivator
	{
		private Manager.CommandManager _commandManager = null;

		public CommandActivator(Manager.CommandManager commandManager)
		{
			_commandManager = commandManager;
		}

		public string Invoke(Query.IRCCommandQuery query)
		{
			return null;
		}
	}
}
