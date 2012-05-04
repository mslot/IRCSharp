using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	internal class PrefixParser : IParsable
	{
		private IRCQueryParser _queryTokensizerParser = null;
		private string _line = String.Empty;

		public PrefixParser(IRCQueryParser queryTokensizerParser)
		{
			_queryTokensizerParser = queryTokensizerParser;
			_line = queryTokensizerParser.Line;
		}

		public int Parse()
		{
			int nextCharCount = 0;
			if (_line.StartsWith(":")) //server sent us an message
			{
				nextCharCount = ParsePrefixString(_line);
			}
			else
			{
				_queryTokensizerParser.Query.Prefix = String.Empty;
			}

			if (_line.Length > 0) //we only want to proceed if the line is not empty
			{
				_queryTokensizerParser.SetCommandParserState();
			}
			else //if the line is empty, then we know we have an error.
			{
				nextCharCount = -1;
			}

			return nextCharCount;
		}

		private int ParsePrefixString(string line)
		{
			_queryTokensizerParser.Query.Prefix = line
				.Substring(
					line.IndexOf(":") + 1,
					GetIndexOfSeperation() - line.IndexOf(":")
				)
				.Trim();

			return GetIndexOfSeperation();
		}

		private int GetIndexOfSeperation()
		{
			return _line.IndexOf(" ");
		}
	}
}
