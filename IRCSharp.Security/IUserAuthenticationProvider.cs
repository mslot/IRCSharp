using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public interface IUserAuthenticationProvider
	{
		bool AuthenticateUser(string userId);
	}
}
