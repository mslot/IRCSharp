using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Model.Query
{
	public class IRCCommandQuery : Commandbase
	{
		public string RawLine { get; set; }
		public string Prefix { get; set; }
		public IRCCommand Command { get; set; }
		public string Parameter { get; set; }

		public IRCCommandQuery()
			: base()
		{

		}

		public IRCCommandQuery(string from, string to)
			: base(from, to)
		{

		}

		public string Nick
		{
			get
			{
				return Prefix;
			}
		}

		public string Channel
		{
			get
			{
				return From;
			}
		}

		public IRCCommandQuery(string line)
			: base(String.Empty, String.Empty)
		{
			RawLine = line;
			Command = IRCCommand.NOT_VALID_COMMAND_TYPE;
			Prefix = String.Empty;
			Parameter = String.Empty;
		}

		public override string ToString()
		{
			return RawLine;
		}
	}
}
