﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Model.Query
{
	public class IRCCommandQuery : Commandbase
	{
		public string RawLine { get; set; }
		public string Prefix { get; set; }
		public ResponseCommand Command { get; set; }
		public string Parameter { get; set; }

		public IRCCommandQuery() : base()
		{

		}

		public IRCCommandQuery(string from, string to) : base(from, to)
		{

		}

		public IRCCommandQuery(string line) : base(String.Empty, String.Empty)
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