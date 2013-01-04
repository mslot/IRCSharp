using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace UnitTest.Security.AuthenticationProvider
{
	[TestFixture]
	public class XMLUserAuthenticationProviderTest
	{
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
		}

		[Test]
		public void TryWithANoneExistantUser()
		{
			bool authenticated = _userProvider.AuthenticateUser("mslot3!~mslot3@56344eba.rev.stofanet.dk");
			Assert.False(authenticated);
		}

		[Test]
		public void TryWithADeniedUser()
		{
			bool authenticated = _userProvider.AuthenticateUser("mslot1!~mslot1@56344eba.rev.stofanet.dk");
			Assert.False(authenticated);
		}

		[Test]
		public void TryWithAuhenticatedUser()
		{
			bool authenticated = _userProvider.AuthenticateUser("mslot2!~mslot2@56344eba.rev.stofanet.dk");
			Assert.True(authenticated);
		}
	}
}
