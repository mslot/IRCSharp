using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public interface ICommandAuthenticationProvider
	{
		bool MayUserWriteToCommand(string user, string commandName, out User commandUser);
		bool MayUserReadFromCommand(string user, string commandName, out User commandUser);
		bool MayUserExecuteCommand(string user, string commandName, out User commandUser);
	}
}
