using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	internal class ParsePrefix : IParsable
	{
		private QueryTokensizerParser _queryTokensizerParser = null;
		private string _line = String.Empty;

		public ParsePrefix(QueryTokensizerParser queryTokensizerParser)
		{
			_queryTokensizerParser = queryTokensizerParser;
			_line = queryTokensizerParser.Line;
		}

		public ParserStatus Parse()
		{
			if (_line.StartsWith(":"))
			{
				_queryTokensizerParser.Query.Prefix = ParsePrefixString(_line);
				_queryTokensizerParser.SetCommandParserState();
				return new ParserStatus { IsError = false, Message = "Prefix found.", Name = "Prefix parsing.", CharCount = GetIndexOfSeperation() };
			}
			else
			{
				_queryTokensizerParser.Query.Prefix = String.Empty;
				_queryTokensizerParser.SetCommandParserState();
				return new ParserStatus { Message = "Prefix not found. Going on to parse command.", Name = "Prefix parsing." };
			}
		}

		private string ParsePrefixString(string line)
		{
			return line
				.Substring(
					line.IndexOf(":") + 1,
					GetIndexOfSeperation() - line.IndexOf(":")
				)
				.Trim();
		}

		private int GetIndexOfSeperation()
		{
			return _line.IndexOf(" ");
		}
	}
}
