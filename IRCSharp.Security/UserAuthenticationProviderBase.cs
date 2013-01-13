using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Security
{
	public abstract class UserAuthenticationProviderBase : IUserAuthenticationProvider
	{
		public bool TryAuthenticateUser(string userId, out User user)
		{
			User foundUser = GetUser(userId);
			bool isAuthenticated = false;
			if (foundUser != null)
			{
				isAuthenticated = foundUser.IsAuthenticated;
			}
			user = foundUser;
			return isAuthenticated;
		}

		public abstract User GetUser(string userId);
	}
}
