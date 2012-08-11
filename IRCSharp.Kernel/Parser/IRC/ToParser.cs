using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	public class ToParser : IParsable
	{
		private IRCParserContext _context = null;

		public ToParser(IRCParserContext context)
		{
			_context = context;
		}

		public int Parse()
		{
			int nextCharCount = ParseToString(_context.Line);
			_context.CurrentState = new ParamsParser(_context);

			return nextCharCount;
		}

		private int ParseToString(string line)
		{
			string to = line.Substring(_context.CharCount+1, _context.CharCount - line.IndexOf(' ')+1);
			_context.Query.To = to;

			return line.IndexOf(' ', _context.CharCount);
		}
	}
}
