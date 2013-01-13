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
		private IRCSharp.Kernel.Security.XMLUserAuthenticationProvider _userProvider;

		[SetUp]
		public void Setup()
		{
			_userProvider = new IRCSharp.Kernel.Security.XMLUserAuthenticationProvider(
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
			IRCSharp.Kernel.Security.User user;
			bool authenticated = _userProvider.TryAuthenticateUser("mslot3!~mslot3@56344eba.rev.stofanet.dk", out user);
			Assert.False(authenticated);
		}

		[Test]
		public void TryWithADeniedUser()
		{
			IRCSharp.Kernel.Security.User user;
			bool authenticated = _userProvider.TryAuthenticateUser("mslot1!~mslot1@56344eba.rev.stofanet.dk", out user);
			Assert.False(authenticated);
		}

		[Test]
		public void TryWithAuhenticatedUser()
		{
			IRCSharp.Kernel.Security.User user;
			bool authenticated = _userProvider.TryAuthenticateUser("mslot2!~mslot2@56344eba.rev.stofanet.dk", out user);
			Assert.True(authenticated);
		}
	}
}
