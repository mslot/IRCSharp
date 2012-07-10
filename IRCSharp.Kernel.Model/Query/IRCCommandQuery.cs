using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Model.Query
{
	public class IRCCommandQuery
	{
		public string RawLine { get; set; }
		public string Prefix { get; set; }
		public ResponseCommand Command { get; set; }
		public string Parameter { get; set; }

		public IRCCommandQuery()
		{

		}

		public IRCCommandQuery(string line)
		{
			RawLine = line;
			Command = ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE;
			Prefix = String.Empty;
			Parameter = String.Empty;
		}

		public override string ToString()
		{
			return RawLine;
		}
	}
}
