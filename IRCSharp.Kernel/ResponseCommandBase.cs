using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public abstract class ResponseCommandBase : ICommand<IRCSharp.Kernel.Query.ResponseCommand, IRCSharp.Kernel.Query.IRCCommandQuery>
	{
		public string Location { get; set; }
		public abstract Query.IRCCommandQuery Execute(IRCSharp.Kernel.Query.IRCCommandQuery query);
		public abstract Query.ResponseCommand Name {get;}
	}
}
