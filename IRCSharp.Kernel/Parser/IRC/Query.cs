using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	public class Query
	{
		public string RawLine { get; private set; }
		public string Prefix { get; set; }
		public ResponseCommand Command { get; set; }
		public string Parameter { get; set; }

		public Query(string line)
		{
			RawLine = line;
			Command = ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE;
			Prefix = String.Empty;
			Parameter = String.Empty;
		}
	}
}
