using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	class ParamsParser : IParsable
	{
		private IRCQueryParser _queryTokensizerParser;
		private string _line = String.Empty;

		public ParamsParser(IRCQueryParser queryTokensizerParser)
		{
			this._queryTokensizerParser = queryTokensizerParser;
			this._line = _queryTokensizerParser.Line;
		}

		public int Parse()
		{
			int nextCharCount = ParseParamsString(_line);

			return nextCharCount;
		}

		private int ParseParamsString(string line)
		{
			int nextCharCount = -1;
			if (_queryTokensizerParser.CharCount != -1)
			{
				string parsedParams = line.Substring(_queryTokensizerParser.CharCount, line.Length - _queryTokensizerParser.CharCount);
				_queryTokensizerParser.Query.Parameter = parsedParams.Trim();
				nextCharCount = line.Length;
			}

			return nextCharCount;
		}
	}
}
