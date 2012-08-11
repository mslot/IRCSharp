using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public abstract class ResponseCommandBase : ICommand<IRCSharp.Kernel.Model.Query.IRCCommand, IRCSharp.Kernel.Model.Query.IRCCommandQuery>
	{
		public string Location { get; set; }
		public abstract Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.IRCCommandQuery query);
		public abstract void Init();
	}
}
