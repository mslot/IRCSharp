using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	public class Query
	{
		public string Prefix { get; set; }

		public CommandType Command { get; set; }

		public string Parameter { get; set; }
	}
}
