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

		public IRCCommandQuery(string from, string to, string network)
			: base(from, to, network)
		{

		}

		public IRCCommandQuery(string line, string network)
			: base(String.Empty, String.Empty, network)
		{
			RawLine = line;
			Command = IRCCommand.NOT_VALID_COMMAND_TYPE;
			Prefix = String.Empty;
			Parameter = String.Empty;
		}

		public string Nick //TODO: should this be parsed by the parser? Maybe a new parser should be added, between PrefixParser and FromParser. See diagrams in kernel projects for more details
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
				return To; //TODO this is not always true. It is only a channel if name is prefixed with #.
			}
		}

		public override string ToString()
		{
			return RawLine;
		}
	}
}
