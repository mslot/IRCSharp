using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.UserdefinedCommand
{
	[TestFixture]
	class UserdefinedParserTest
	{
		[Test]
		public void QueryParseTestParsingUserdefinedCommand()
		{
			string line = ":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3";
			IRCSharp.Kernel.Parser.IRC.Query ircQuery = null;
			bool parsedIRCQuery = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out ircQuery);

			Assert.True(parsedIRCQuery);

			IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandQuery userdefinedCommandQuery;
			bool parsedUserdefinedCommand = IRCSharp.Kernel.Parser.UserdefinedCommand.UserdefinedCommandParser.TryParse(ircQuery, out userdefinedCommandQuery);

			Assert.True(parsedUserdefinedCommand);

		}
	}
}
