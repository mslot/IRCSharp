using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.UserdefinedCommand
{
	public class UserdefinedCommandParser
	{
		private UserdefinedCommandParser()
		{

		}

		public static bool TryParse(IRCSharp.Kernel.Parser.IRC.Query ircQuery, out UserdefinedCommandQuery userdefinedCommandQuery)
		{
			UserdefinedCommandParser parser = new UserdefinedCommandParser();
			bool parsed = false;
			UserdefinedCommandQuery query = null;
			if (ircQuery.Command == IRC.ResponseCommand.PRIVMSG)
			{
				string commandName = parser.ParseCommandName(ircQuery.Parameter);
				IList<string> commandNameParameters = parser.ParseCommandParameters(ircQuery.Parameter);
				userdefinedCommandQuery = new UserdefinedCommandQuery(commandName, ircQuery);
				userdefinedCommandQuery.AddParameters(commandNameParameters);
				parsed = true;
			}

			userdefinedCommandQuery = query;
			return parsed;
		}

		private string ParseCommandName(string ircQueryParameter)
		{
			throw new NotImplementedException();
		}

		private IList<string> ParseCommandParameters(string ircQueryParameter)
		{
			throw new NotImplementedException();
		}
	}
}
