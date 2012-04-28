using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public abstract class UserdefinedCommandBase : ICommand<string, IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery>
	{
		public abstract string Execute(IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery query);
		public abstract string Name { get; }
		public string Location { get; set; }

		public UserdefinedCommandBase()
		{

		}
	}
}
