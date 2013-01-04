using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public interface IAuthenticationProvider : IUserAuthenticationProvider, ICommandAuthenticationProvider
	{
	}
}
