using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.RavelDalTest
{
	[TestFixture]
	class AddQueryTest
	{
		public IEnumerable<IRCSharp.Kernel.Model.Query.IRCCommandQuery> GenerateQueries()
		{
			LinkedList<IRCSharp.Kernel.Model.Query.IRCCommandQuery> queries = new LinkedList<IRCSharp.Kernel.Model.Query.IRCCommandQuery>();
			List<string> rawQueries = new List<string> {
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot1!~mslot1@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #c :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #c :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #c :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #c :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #c :!command arg1 arg2 arg3",
				":mslot2!~mslot2@56344eba.rev.stofanet.dk PRIVMSG #c :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #c :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #r :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot3!~mslot3@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #lol :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #3 :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #3 :!command arg1 arg2 arg3",
				":mslot4!~mslot4@56344eba.rev.stofanet.dk PRIVMSG #3 :!command arg1 arg2 arg3",
				":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #error :!command arg1 arg2 arg3",
				":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #error :!command arg1 arg2 arg3",
				":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #error :!command arg1 arg2 arg3",
				":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #error :!command arg1 arg2 arg3"
			};

			foreach (string rawQuery in rawQueries)
			{
				IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
				bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(rawQuery, out query);
				queries.AddLast(query);
			}

			return queries;
		}


		[Test]
		public void AddFirstGeneratedQueries()
		{
			var generatedQueries = GenerateQueries();
			IRCSharp.Kernel.Model.Query.IRCCommandQuery firstQuery = generatedQueries.First();

		}
	}
}
