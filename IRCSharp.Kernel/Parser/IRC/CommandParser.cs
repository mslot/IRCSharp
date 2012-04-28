using System;
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

		public ParserStatus Parse()
		{
			ParserStatus parserStatus = new ParserStatus { Name = "parsing command success.", Message = "parsed following command: ", Exception = null };
			_queryTokensizerParser.Query.Command = ParseResponseCommand(_line, ref parserStatus);
			_queryTokensizerParser.SetParseParamsState();

			return parserStatus;
		}

		private ResponseCommand ParseResponseCommand(string line, ref ParserStatus parserStatus)
		{
			ResponseCommand responseCommand = ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE;

			if (!String.IsNullOrEmpty(line))
			{
				//look between CharCount and next space for the command type
				int charCount = GetCharCount(_queryTokensizerParser.ParserStatus.CharCount);
				int nextWhitespace = line.IndexOf(" ", charCount);
				int length = nextWhitespace - _queryTokensizerParser.ParserStatus.CharCount;
				string command = line.Substring(charCount, length).Trim(); 
				responseCommand = DetermineResponseCommand(command, ref parserStatus);
				parserStatus.CharCount = nextWhitespace;
			}
			else
			{
				parserStatus = new ParserStatus { Exception = new Exception("The command received is not an official IRC command."), IsError = true, Message = "Failed parsing command.", Name = "failed parsing command." };
			}

			return responseCommand;
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

		private ResponseCommand DetermineResponseCommand(string command, ref ParserStatus parserStatus)
		{
			ResponseCommand message = default(ResponseCommand);
			switch (command)
			{
				case "422": message = ResponseCommand.ERR_NOMOTD; break;
				case "375": message = ResponseCommand.RPL_MOTDSTART; break;
				case "372": message = ResponseCommand.RPL_MOTD; break;
				case "376": message = ResponseCommand.RPL_ENDOFMOTD; break;
				case "221": message = ResponseCommand.RPL_UMODEIS; break;
				case "005": message = ResponseCommand.RPL_ISUPPORT; break; //defacto standard http://www.mirc.com/isupport.html, not official. Implemented by Undernet software
				case "PRIVMSG": message = ResponseCommand.PRIVMSG; break;
				case "PING": message = ResponseCommand.PING; break;
				case "JOIN": message = ResponseCommand.JOIN; break;
				default: parserStatus = new ParserStatus { Exception = new Exception("The command received is not an official IRC command."), IsError = true, Message = "Failed parsing command.", Name = "failed parsing command." }; break;
			}

			if (!parserStatus.IsError)
			{
				parserStatus.Message += " " + command;
			}

			if (message == ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE)
			{
				parserStatus = new ParserStatus { Exception = new Exception("The command received is not an official IRC command."), IsError = true, Message = "Failed parsing command.", Name = "failed parsing command." };
			}

			return message;
		}
	}
}
