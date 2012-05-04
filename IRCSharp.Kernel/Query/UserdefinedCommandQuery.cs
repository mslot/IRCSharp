using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Query
{
	public class UserdefinedCommandQuery
	{
		private IRCCommandQuery ircQuery;

		public string From { get; private set;}
		public string To { get; private set; }
		public IRCCommandQuery IRCQuery { get; private set; }
		public List<string> Parameters { get; private set; }
		public string CommandName { get; private set; }

		public UserdefinedCommandQuery(string to, string from, string commandName, Query.IRCCommandQuery ircQuery)
		{
			From = from;
			To = to;
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
