using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.SQLiteDalTest
{
	class UserMslot1DataTest : QueryTestBase
	{
		[Test]
		public void TestUserMslot1Data()
		{
			var generatedQueries = GenerateQueries();

			foreach (var query in generatedQueries)
			{
				bool added = base.Dal.AddQuery(query);
				Assert.IsTrue(added);
			}

			IRCSharp.Statistics.Kernel.Model.User user = base.Dal.GetUser("network", "mslot1!~mslot1@56344eba.rev.stofanet.dk", 100);
			Assert.NotNull(user);

			Assert.AreEqual(1, user.Channels.Count());
			Assert.AreEqual(8, user.Channels.First().Queries.Count());
		}
	}
}
