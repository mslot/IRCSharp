using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public abstract class UserAuthenticationProviderBase : IUserAuthenticationProvider
	{
		public bool AuthenticateUser(string userId)
		{
			User foundUser = GetUser(userId);
			bool isAuthenticated = false;
			if (foundUser != null)
			{
				isAuthenticated = foundUser.IsAuthenticated;
			}

			return isAuthenticated;
		}

		public abstract User GetUser(string userId);
	}
}
