using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	class ParamsParser : IParsable
	{
		private IRCParserContext _context;
		private string _line = String.Empty;

		public ParamsParser(IRCParserContext _context)
		{
			this._context = _context;
			this._line = _context.Line;
		}

		public int Parse()
		{
			int nextCharCount = ParseParamsString(_line);

			return nextCharCount;
		}

		private int ParseParamsString(string line)
		{
			int nextCharCount = -1;
			if (_context.CharCount != -1)
			{
				string parsedParams = line.Substring(_context.CharCount, line.Length - _context.CharCount);
				_context.Query.Parameter = parsedParams.Trim();
				nextCharCount = line.Length;
			}

			return nextCharCount;
		}
	}
}
