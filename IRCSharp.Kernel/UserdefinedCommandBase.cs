using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public abstract class UserdefinedCommandBase : ICommand<string, IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery>
	{
		public abstract Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery query);
		public string Location { get; set; }

		public UserdefinedCommandBase()
		{

		}

		public abstract void Init();
	}
}
