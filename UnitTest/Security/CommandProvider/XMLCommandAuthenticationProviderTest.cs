using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.Security.AuthenticationProvider
{
	[TestFixture]
	public class XMLCommandAuthenticationProviderTest
	{
		private IRCSharp.Security.XMLCommandAuthenticationProvider _commandProvider;
		private IRCSharp.Security.XMLUserAuthenticationProvider _userProvider;

		[SetUp]
		public void Setup()
		{
			_userProvider = new IRCSharp.Security.XMLUserAuthenticationProvider(
				new System.IO.StreamReader(
					new System.IO.MemoryStream(
						System.Text.Encoding.UTF8.GetBytes(XMLAuthenticationProviderData.UserList)
					)
				)
			);
			_commandProvider = new IRCSharp.Security.XMLCommandAuthenticationProvider(
				_userProvider
			);
		}

		[Test]
		public void TryWithFalseWrite()
		{
			IRCSharp.Security.User user;
			bool mayUserWrite = _commandProvider.MayUserWriteToCommand("mslot1!~mslot1@56344eba.rev.stofanet.dk", "commandName1", out user);
			Assert.IsFalse(mayUserWrite);
			Assert.True(user.Commands.Count == 2);
			Assert.True(user.Commands.First().Name == "commandName1");
			
		}

		[Test]
		public void TryWithTrueRead()
		{
			IRCSharp.Security.User user;
			bool mayUserRead = _commandProvider.MayUserReadFromCommand("mslot1!~mslot1@56344eba.rev.stofanet.dk", "commandName1", out user);
			Assert.IsFalse(mayUserRead);
			Assert.True(user.Commands.Count == 2);
			Assert.True(user.Commands.First().Name == "commandName1");

		}

		[Test]
		public void TryWithTrueExecute()
		{
			IRCSharp.Security.User user;
			bool mayUserExecute = _commandProvider.MayUserExecuteCommand("mslot2!~mslot2@56344eba.rev.stofanet.dk", "commandName1", out user);
			bool mayUserRead = _commandProvider.MayUserReadFromCommand("mslot2!~mslot2@56344eba.rev.stofanet.dk", "commandName1", out user);
			bool mayUserWrite = _commandProvider.MayUserWriteToCommand("mslot2!~mslot2@56344eba.rev.stofanet.dk", "commandName1", out user);
			Assert.IsTrue(mayUserExecute);
			Assert.IsTrue(mayUserRead);
			Assert.IsTrue(mayUserWrite);
			Assert.True(user.Commands.Count == 2);
			Assert.True(user.Commands.First().Name == "commandName1");

		}

	}
}
