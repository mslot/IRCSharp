using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.UserdefinedCommand
{
	public class UserdefinedCommandQuery
	{
		public Parser.IRC.Query IRCQuery { get; private set; }
		public List<string> Parameters { get; private set; }
		public string CommandName { get; private set; }

		public UserdefinedCommandQuery( string commandName, Parser.IRC.Query ircQuery)
		{
			CommandName = commandName;
			IRCQuery = ircQuery;
			Parameters = new List<string>();
		}

		public void AddParameter(string parameter)
		{
			Parameters.Add(parameter);
		}

		public void AddParameters(IList<string> commandParameters)
		{
			Parameters.AddRange(commandParameters);
		}
	}
}
