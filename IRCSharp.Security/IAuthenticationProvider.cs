using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Security
{
	public interface IAuthenticationProvider : IUserAuthenticationProvider, ICommandAuthenticationProvider
	{
	}
}
