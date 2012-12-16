using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.SQLiteDalTest
{
	class UserMslot2DataTest : QueryTestBase
	{
		[Test]
		public void TestUserMslot2Data()
		{
			var generatedQueries = GenerateQueries();

			foreach (var query in generatedQueries)
			{
				bool added = base.Dal.AddQuery(query);
				Assert.IsTrue(added);
			}

			IRCSharp.Statistics.Kernel.Model.User user = base.Dal.GetUser("network", "mslot2!~mslot2@56344eba.rev.stofanet.dk", 100);
			
			Assert.NotNull(user);
			Assert.AreEqual(2, user.Channels.Count());
			Assert.AreEqual("#mslot.dk", user.Channels.First().Name);
			Assert.AreEqual(7, user.Channels.First().Queries.Count());
		}
	}
}
