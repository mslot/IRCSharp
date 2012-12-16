using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Model.Query
{
	public class UserdefinedCommandQuery
	{
		public IRCCommandQuery IRCQuery { get; private set; }
		public List<string> Parameters { get; private set; }
		public string CommandName { get; private set; }
		public string From { get { return IRCQuery.From; } }
		public string To { get { return IRCQuery.To; } }
		public string Network { get { return IRCQuery.Network; } }

		public UserdefinedCommandQuery(string commandName, Query.IRCCommandQuery ircQuery)
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
