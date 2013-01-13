using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Security
{
	public class AuthenticationProvider : IAuthenticationProvider
	{
		private ICommandAuthenticationProvider _commandAuthenticationProvider;
		private IUserAuthenticationProvider _userAuthenticationProvider;

		public AuthenticationProvider(ICommandAuthenticationProvider commandAuthenticationProvider, IUserAuthenticationProvider userAuthenticationProvider)
		{
			_commandAuthenticationProvider = commandAuthenticationProvider;
			_userAuthenticationProvider = userAuthenticationProvider;
		}

		public bool TryAuthenticateUser(string userId, out User user)
		{
			return _userAuthenticationProvider.TryAuthenticateUser(userId, out user);
		}

		public bool MayUserWriteToCommand(string user, string commandName, out User commandUser)
		{
			return _commandAuthenticationProvider.MayUserExecuteCommand(user, commandName, out commandUser);
		}

		public bool MayUserReadFromCommand(string user, string commandName, out User commandUser)
		{
			return _commandAuthenticationProvider.MayUserReadFromCommand(user, commandName, out commandUser);
		}

		public bool MayUserExecuteCommand(string user, string commandName, out User commandUser)
		{
			return _commandAuthenticationProvider.MayUserExecuteCommand(user, commandName, out commandUser);
		}
	}
}
