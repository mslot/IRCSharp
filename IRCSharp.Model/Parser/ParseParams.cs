using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	class ParseParams : IParsable
	{
		private QueryTokensizerParser _queryTokensizerParser;
		private string _line = String.Empty;

		public ParseParams(QueryTokensizerParser queryTokensizerParser)
		{
			this._queryTokensizerParser = queryTokensizerParser;
			this._line = _queryTokensizerParser.Line;
		}

		public ParserStatus Parse()
		{
			ParserStatus parserStatus = new ParserStatus();
			string parsedParams = ParseParamsString(_line, out parserStatus);
			_queryTokensizerParser.Query.Parameter = parsedParams.Trim();

			return parserStatus;
		}

		private string ParseParamsString(string line, out ParserStatus parserStatus)
		{
			string parsedParams = line.Substring(_queryTokensizerParser.ParserStatus.CharCount, line.Length - _queryTokensizerParser.ParserStatus.CharCount);
			parserStatus = new ParserStatus { IsError = false, Done = true, CharCount = line.Length, Exception = null, Message = "Done parsing params.", Name = "Parsed params." };

			return parsedParams;
		}
	}
}
