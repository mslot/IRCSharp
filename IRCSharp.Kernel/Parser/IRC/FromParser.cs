using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	public class FromParser : IParsable
	{
		private IRCParserContext _context = null;
		private string _line = String.Empty;

		public FromParser(IRCParserContext context)
		{
			_line = context.Line;
			_context = context;
		}

		public int Parse()
		{
			int nextCharCount = ParsePrefixString(_line);
			_context.CurrentState = new CommandParser(_context);

			return nextCharCount;
		}

		private int GetIndexOfSeperation()
		{
			return _line.IndexOf(" ");
		}

		private int ParsePrefixString(string line)
		{
			_context.Query.From = line
				.Substring(
					line.IndexOf(":") + 1,
					GetIndexOfSeperation() - line.IndexOf(":")
				)
				.Trim();

			return GetIndexOfSeperation();
		}
	}
}
