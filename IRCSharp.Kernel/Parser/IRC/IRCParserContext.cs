using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	public class IRCParserContext
	{
		public IParsable CurrentState { get; set; }
		public String Line { get; set; }
		public Model.Query.IRCCommandQuery Query { get; set; }
		public int CharCount { get; set; }
	}
}
