using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	class CommandParser : IParsable
	{
		private IRCParserContext _context= null;
		private string _line = String.Empty;

		public CommandParser(IRCParserContext context)
		{
			_context = context;
			_line = _context.Line;
		}
		
		public int Parse()
		{
			int nextCharCount = ParseResponseCommand(_line);
			if (_line[nextCharCount + 1] == ':')
			{
				_context.CurrentState = new ParamsParser(_context);
			}
			else
			{
				_context.CurrentState = new ToParser(_context);
			}

			return nextCharCount;
		}

		private int ParseResponseCommand(string line)
		{
			Model.Query.ResponseCommand responseCommand = Model.Query.ResponseCommand.NOT_VALID_RESPONSE_COMMAND_TYPE;
			int nextCharCount = -1;
			if (!String.IsNullOrEmpty(line))
			{
				//look between CharCount and next space for the command type
				int charCount = GetCharCount(_context.CharCount);
				int nextWhitespace = line.IndexOf(" ", charCount);
				int length = nextWhitespace - _context.CharCount;
				string command = line.Substring(charCount, length).Trim();
				responseCommand = DetermineResponseCommand(command);
				_context.Query.Command = responseCommand;
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

		private Model.Query.ResponseCommand DetermineResponseCommand(string command)
		{
			Model.Query.ResponseCommand message = default(Model.Query.ResponseCommand);
			switch (command)
			{
				case "422": message = Model.Query.ResponseCommand.ERR_NOMOTD; break;
				case "375": message = Model.Query.ResponseCommand.RPL_MOTDSTART; break;
				case "372": message = Model.Query.ResponseCommand.RPL_MOTD; break;
				case "376": message = Model.Query.ResponseCommand.RPL_ENDOFMOTD; break;
				case "221": message = Model.Query.ResponseCommand.RPL_UMODEIS; break;
				case "005": message = Model.Query.ResponseCommand.RPL_ISUPPORT; break; //defacto standard http://www.mirc.com/isupport.html, not official. Implemented by Undernet software
				case "PRIVMSG": message = Model.Query.ResponseCommand.PRIVMSG; break;
				case "PING": message = Model.Query.ResponseCommand.PING; break;
				case "JOIN": message = Model.Query.ResponseCommand.JOIN; break;
			}

			return message;
		}
	}
}
