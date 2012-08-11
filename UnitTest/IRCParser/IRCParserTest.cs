using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.IRCParser
{
	[TestFixture]
	class ParserTest
	{
		/* This is an example of a IRC message to a client
		 * :mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3
		 * 
		 * According to http://www.irchelp.org/irchelp/rfc/chapter2.html#c2_3
		 * : == static char
		 * mslot!~mslot@56344eba.rev.stofanet.dk == prefix
		 * PRIVMSG == command
		 * params == #mslot.dk :!command arg1 arg2 arg3
		 * #mslot.dk == params[middle] (and should be interpered as target)
		 * :!command arg1 arg2 arg3 == params[trailing]
		 * 
		 */

		[Test]
		public void QueryParseTestWithOneTargetAndTrailngParams()
		{
			string line = ":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :!command arg1 arg2 arg3";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("mslot!~mslot@56344eba.rev.stofanet.dk",query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.PRIVMSG, query.Command);
			Assert.AreEqual("#mslot.dk :!command arg1 arg2 arg3", query.Parameter);
			Assert.AreEqual("mslot!~mslot@56344eba.rev.stofanet.dk", query.From);
			Assert.AreEqual("#mslot.dk", query.To);
			Assert.True(parsed);

		}

		[Test]
		public void QueryParseTestWithOneTargetAndNoTrailing()
		{
			string line = ":mslot!~mslot@56344eba.rev.stofanet.dk PRIVMSG #mslot.dk :";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("mslot!~mslot@56344eba.rev.stofanet.dk", query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.PRIVMSG, query.Command);
			Assert.AreEqual("#mslot.dk :", query.Parameter);
			Assert.True(parsed);
		}

		[Test]
		public void QueryParseTestWithNoPreifxAndNoTrailing()
		{
			string line = "PING :123234";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.PING, query.Command);
			Assert.AreEqual(":123234", query.Parameter);
			Assert.True(parsed);

		}

		[Test]
		public void QueryParseTestWithNoPreifxAndNoTrailingPONG()
		{
			string line = ":hej@med.dig PING 123234";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("hej@med.dig", query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.PING, query.Command);
			Assert.AreEqual("123234", query.Parameter);
			Assert.True(parsed);

		}

		[Test]
		public void QueryParseTestWithJoin()
		{
			string line = ":WiZ JOIN #Twilight_zone";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("WiZ", query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.JOIN, query.Command);
			Assert.AreEqual("#Twilight_zone", query.Parameter);
			Assert.True(parsed);

		}

		[Test]
		public void QueryParseTestWithJoinClientSide()
		{
			string line = ":WiZ JOIN #foo,#bar fubar,foobar";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("WiZ", query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.JOIN, query.Command);
			Assert.AreEqual("#foo,#bar fubar,foobar", query.Parameter);
			Assert.True(parsed);

		}

		[Test]
		public void QueryParseTesBlank()
		{
			string line = "";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("", query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.NOT_VALID_RESPONSE_COMMAND_TYPE, query.Command);
			Assert.AreEqual("", query.Parameter);
			Assert.False(parsed);

		}

		[Test]
		public void RandomQueryTest()
		{
			string line = "PRIVMSG #mslot.dk :kingkong";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("", query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.PRIVMSG, query.Command);
			Assert.AreEqual("#mslot.dk :kingkong", query.Parameter);
			Assert.True(parsed);

		}

		[Test]
		public void QueryParseNUmericReply005()
		{
			string line = ":jubii.dk.quakenet.org 005 ro-bertbot MAXNICKLEN=15 TOPICLEN=250 AWAYLEN=160 KICKLEN=250 CHANNELLEN=200 MAXCHANNELLEN=200 CHANTYPES=#& PREFIX=(ov)@+ STATUSMSG=@+ CHANMODES=b,k,l,imnpstrDducCNMT CASEMAPPING=rfc1459 NETWORK=QuakeNet :are supported by this server";
			IRCSharp.Kernel.Model.Query.IRCCommandQuery query = null;
			bool parsed = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(line, out query);

			Assert.AreEqual("jubii.dk.quakenet.org", query.Prefix);
			Assert.AreEqual(IRCSharp.Kernel.Model.Query.IRCCommand.RPL_ISUPPORT, query.Command);
			Assert.AreEqual("ro-bertbot MAXNICKLEN=15 TOPICLEN=250 AWAYLEN=160 KICKLEN=250 CHANNELLEN=200 MAXCHANNELLEN=200 CHANTYPES=#& PREFIX=(ov)@+ STATUSMSG=@+ CHANMODES=b,k,l,imnpstrDducCNMT CASEMAPPING=rfc1459 NETWORK=QuakeNet :are supported by this server", query.Parameter);
			Assert.True(parsed);

		}

	}
}
 