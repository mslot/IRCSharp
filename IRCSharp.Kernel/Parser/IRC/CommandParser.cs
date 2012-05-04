﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	class CommandParser : IParsable
	{
		private IRCQueryParser _queryTokensizerParser = null;
		private string _line = String.Empty;

		public CommandParser(IRCQueryParser queryTokensizerParser)
		{
			this._queryTokensizerParser = queryTokensizerParser;
			_line = queryTokensizerParser.Line;
		}

		public int Parse()
		{
			int nextCharCount = ParseResponseCommand(_line);
			_queryTokensizerParser.SetParseParamsState();

			return nextCharCount;
		}

		private int ParseResponseCommand(string line)
		{
			Query.ResponseCommand responseCommand = Query.ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE;
			int nextCharCount = -1;
			if (!String.IsNullOrEmpty(line))
			{
				//look between CharCount and next space for the command type
				int charCount = GetCharCount(_queryTokensizerParser.CharCount);
				int nextWhitespace = line.IndexOf(" ", charCount);
				int length = nextWhitespace - _queryTokensizerParser.CharCount;
				string command = line.Substring(charCount, length).Trim();
				responseCommand = DetermineResponseCommand(command);
				_queryTokensizerParser.Query.Command = responseCommand;
				nextCharCount  = nextWhitespace;
			}

			return nextCharCount;
		}

		private int GetCharCount(int charCount)
		{
			if (charCount == 0)
			{
				return 0;
			}
			else
			{
				return charCount + 1;
			}
		}

		private Query.ResponseCommand DetermineResponseCommand(string command)
		{
			Query.ResponseCommand message = default(Query.ResponseCommand);
			switch (command)
			{
				case "422": message = Query.ResponseCommand.ERR_NOMOTD; break;
				case "375": message = Query.ResponseCommand.RPL_MOTDSTART; break;
				case "372": message = Query.ResponseCommand.RPL_MOTD; break;
				case "376": message = Query.ResponseCommand.RPL_ENDOFMOTD; break;
				case "221": message = Query.ResponseCommand.RPL_UMODEIS; break;
				case "005": message = Query.ResponseCommand.RPL_ISUPPORT; break; //defacto standard http://www.mirc.com/isupport.html, not official. Implemented by Undernet software
				case "PRIVMSG": message = Query.ResponseCommand.PRIVMSG; break;
				case "PING": message = Query.ResponseCommand.PING; break;
				case "JOIN": message = Query.ResponseCommand.JOIN; break;
			}

			return message;
		}
	}
}
