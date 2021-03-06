﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	internal class PrefixParser : IParsable
	{
		private IRCParserContext _context = null;
		private string _line = String.Empty;

		public PrefixParser(IRCParserContext context)
		{
			_context = context;
		}

		public int Parse()
		{
			_line = _context.Line;
			int nextCharCount = 0;
			bool isIncoming = false;
			if (_line != null && _line.StartsWith(":")) //server sent us an message
			{
				ParsePrefixString(_line);
				isIncoming = true;
			}
			else
			{
				//maybe the query line is a IRC command that is sent, not received?
				_context.Query.From = String.Empty;
				_context.Query.Prefix = String.Empty;
			}

			if (_line.Length > 0) //we only want to proceed if the line is not empty
			{
				if (isIncoming)
				{
					_context.CurrentState = new FromParser(_context);
				}
				else
				{
					_context.CurrentState = new CommandParser(_context);
				}

			}
			else //if the line is empty, then we know we have an error.
			{
				nextCharCount = -1;
			}

			return nextCharCount;
		}

		private int GetIndexOfSeperation()
		{
			return _line.IndexOf(" ");
		}

		private int ParsePrefixString(string line)
		{
			_context.Query.Prefix = line
				.Substring(
					line.IndexOf(":") + 1,
					GetIndexOfSeperation() - line.IndexOf(":")
				)
				.Trim();

			return GetIndexOfSeperation();
		}

	}
}
