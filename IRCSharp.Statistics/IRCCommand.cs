using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics
{
	[IRCSharp.Kernel.IRCCommand(IRCSharp.Kernel.Model.Query.IRCCommand.ALL)]
	public class StatisticsIRCCommand : IRCSharp.Kernel.ResponseCommandBase
	{

		public StatisticsIRCCommand()
		{
		}

		public override IRCSharp.Kernel.Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
	
			return null;
		}


		public override void Init()
		{
		}
	}
}
