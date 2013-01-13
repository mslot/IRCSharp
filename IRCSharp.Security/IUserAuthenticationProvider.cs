using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Security
{
	public interface IUserAuthenticationProvider
	{
		//TODO: maybe we should provide an extra method like "GetUser" that just returns the user
		bool TryAuthenticateUser(string userId, out User user);
	}
}
