using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Dal
{
	public interface IDal : IDisposable
	{
		bool AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query);
		Model.User GetUser(string network, string nick, int numberOfQueries);
		void RemoveAllQueries();

		void RemoveAllChannels();

		void RemoveUsers();
	}
}
