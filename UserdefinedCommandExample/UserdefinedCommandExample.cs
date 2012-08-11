using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserdefinedCommandExample
{
	[IRCSharp.Kernel.UserdefinedCommand("hello")]
	public class UserdefinedCommandExample : IRCSharp.Kernel.UserdefinedCommandBase
	{
		public override IRCSharp.Kernel.Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery query)
		{
			Console.WriteLine("hello from command");
			return null;
		}

		public override string Name
		{
			get
			{
				return "hello";
			}
		}

		public override void Init()
		{
			//here we can run code that should only run once, when the command is initiated. This happens when the bot is starting up.
		}
	}
}
