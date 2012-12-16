using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnitTest.SQLiteDalTest;

namespace UnitTest.SQLiteDal
{
	[TestFixture]
	class AddQueryTest : QueryTestBase
	{
		[Test]
		// Tests is run on this query: :mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3
		public void AddFirstGeneratedQuery()
		{
			var generatedQueries = GenerateQueries();
			IRCSharp.Kernel.Model.Query.IRCCommandQuery firstQuery = generatedQueries.First();
			bool added = base.Dal.AddQuery(firstQuery);

			Assert.True(added);

			IRCSharp.Statistics.Kernel.Model.User user = base.Dal.GetUser("network", "mslot1!~mslot1@56344eba.rev.stofanet.dk",10);

			Assert.NotNull(user);
			Assert.AreEqual(1, user.Channels.First().Queries.Count);
		}

		[Test]
		public void AddAllQueries()
		{
			var generatedQueries = GenerateQueries();

			foreach (var query in generatedQueries)
			{
				bool added = base.Dal.AddQuery(query);
				Assert.IsTrue(added);
			}
		}
	}
}
