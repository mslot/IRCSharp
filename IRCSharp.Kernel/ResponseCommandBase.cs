using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public abstract class ResponseCommandBase : ICommand<IRCSharp.Kernel.Parser.IRC.ResponseCommand, IRCSharp.Kernel.Parser.IRC.Query>
	{
		public string Location { get; set; }
		public abstract string Execute(IRCSharp.Kernel.Parser.IRC.Query query);
		public abstract Parser.IRC.ResponseCommand Name {get;}
	}
}
