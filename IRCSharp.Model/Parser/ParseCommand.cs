using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	class ParseCommand : IParsable
	{
		private QueryTokensizerParser _queryTokensizerParser = null;
		private string _line = String.Empty;

		public ParseCommand(QueryTokensizerParser queryTokensizerParser)
		{
			this._queryTokensizerParser = queryTokensizerParser;
			_line = queryTokensizerParser.Line;
		}

		public ParserStatus Parse()
		{
			ParserStatus parserStatus = new ParserStatus { Name = "parsing command success.", Message = "parsed following command: ", Exception = null };
			_queryTokensizerParser.Query.Command = ParseCommandType(_line, ref parserStatus);
			_queryTokensizerParser.SetParseParamsState();

			return parserStatus;
		}

		private CommandType ParseCommandType(string line, ref ParserStatus parserStatus)
		{
			//look between CharCount and next space for the command type
			int nextWhitespace = line.IndexOf(" ", _queryTokensizerParser.ParserStatus.CharCount + 1);
			int length = nextWhitespace - _queryTokensizerParser.ParserStatus.CharCount;
			int charCount = GetCharCount(_queryTokensizerParser.ParserStatus.CharCount);

			string command = line.Substring(charCount, length).Trim();
			CommandType commandType = DetermineCommandType(command, ref parserStatus);
			parserStatus.CharCount = nextWhitespace;

			return commandType;
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

		private CommandType DetermineCommandType(string command, ref ParserStatus parserStatus)
		{
			CommandType message = default(CommandType);
			switch (command)
			{
				case "PASS": message = CommandType.PASS; break;
				case "NICK": message = CommandType.NICK; break;
				case "USER": message = CommandType.USER; break;
				case "SERVER": message = CommandType.SERVER; break;
				case "OPER": message = CommandType.OPER; break;
				case "SQUIT": message = CommandType.SQUIT; break;
				case "JOIN": message = CommandType.JOIN; break;
				case "PART": message = CommandType.PART; break;
				case "MODE": message = CommandType.MODE; break;
				case "TOPIC": message = CommandType.TOPIC; break;
				case "NAMES": message = CommandType.NAMES; break;
				case "LIST": message = CommandType.LIST; break;
				case "INVITE": message = CommandType.INVITE; break;
				case "KICK": message = CommandType.KICK; break;
				case "VERSION": message = CommandType.VERSION; break;
				case "STATS": message = CommandType.STATS; break;
				case "LINKS": message = CommandType.LINKS; break;
				case "TIME": message = CommandType.TIME; break;
				case "TRACE": message = CommandType.TRACE; break;
				case "ADMIN": message = CommandType.ADMIN; break;
				case "PRIVMSG": message = CommandType.PRIVMSG; break;
				case "NOTICE": message = CommandType.NOTICE; break;
				case "WHOIS": message = CommandType.WHOIS; break;
				case "WHOWAS": message = CommandType.WHOWAS; break;
				case "KILL": message = CommandType.KILL; break;
				case "PING": message = CommandType.PING; break;
				case "PONG": message = CommandType.PONG; break;
				case "ERROR": message = CommandType.PONG; break;
				default: parserStatus = new ParserStatus { Exception = new Exception("The command received is not an official IRC command."), IsError = true, Message = "Failed parsing command.", Name = "failed parsing command." }; break;
			}

			if (!parserStatus.IsError)
			{
				parserStatus.Message += " " + command;
			}

			return message;
		}
	}
}
