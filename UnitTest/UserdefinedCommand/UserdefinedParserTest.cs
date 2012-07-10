using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.UserdefinedCommand
{
	//:mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!hello
	[TestFixture]
	class UserdefinedParserTest
	{
		[Test]
		public void QueryParseTestParsingUserdefinedCommand()
		{
			string line = ":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery ircQuery = null;
			bool parsedIRCQuery = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out ircQuery);

			Assert.True(parsedIRCQuery);

			IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery userdefinedCommandQuery;
			bool parsedUserdefinedCommand = IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(ircQuery, out userdefinedCommandQuery);

			Assert.NotNull(userdefinedCommandQuery);
			Assert.True(parsedUserdefinedCommand);
			Assert.AreEqual("mslot", userdefinedCommandQuery.From);
			Assert.AreEqual("#mslot.dk", userdefinedCommandQuery.To);
			Assert.AreEqual("command", userdefinedCommandQuery.CommandName);
			Assert.Contains("arg1", userdefinedCommandQuery.Parameters);
			Assert.Contains("arg2", userdefinedCommandQuery.Parameters);
			Assert.Contains("arg3", userdefinedCommandQuery.Parameters);

		}

		[Test]
		public void QueryParseTestParsingUserdefinedCommandNoParams()
		{
			string line = ":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!hello";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery ircQuery = null;
			bool parsedIRCQuery = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out ircQuery);

			Assert.True(parsedIRCQuery);

			IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery userdefinedCommandQuery;
			bool parsedUserdefinedCommand = IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(ircQuery, out userdefinedCommandQuery);

			Assert.NotNull(userdefinedCommandQuery);
			Assert.True(parsedUserdefinedCommand);
			Assert.AreEqual("mslot", userdefinedCommandQuery.From);
			Assert.AreEqual("#mslot.dk", userdefinedCommandQuery.To);
			Assert.AreEqual("hello", userdefinedCommandQuery.CommandName);

		}
	}
}
