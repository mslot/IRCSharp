using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Security
{
	///TODO: this class should, strickly speaking, load from an xml document, but I am in no need of that now
	///		 and therefore I put this responsability on the UserAuthenticationProvider.
	public class XMLCommandAuthenticationProvider : ICommandAuthenticationProvider
	{
		private IUserAuthenticationProvider _userAuthenticationProvider = null;

		public XMLCommandAuthenticationProvider(IUserAuthenticationProvider userAuthenticationProvider)
		{
			_userAuthenticationProvider = userAuthenticationProvider;
		}

		public bool MayUserWriteToCommand(string username, string commandName, out User commandUser)
		{
			bool isUserAuthenticated = false;
			bool mayUserWrite = false;
			User user;
			if (isUserAuthenticated = _userAuthenticationProvider.TryAuthenticateUser(username, out user))
			{
				Command command = user.Commands.Where(selectedCommand => selectedCommand.Name == commandName).FirstOrDefault();
				if (command != null)
				{
					if (command.Rights.Any(right => right.RightType == CommandRightType.Write))
					{
						mayUserWrite = true;
					}
				}
			}

			commandUser = user;
			return isUserAuthenticated && mayUserWrite;
		}

		public bool MayUserReadFromCommand(string username, string commandName, out User commandUser)
		{
			bool isUserAuthenticated = false;
			bool mayUserRead = false;
			User user;
			if (isUserAuthenticated = _userAuthenticationProvider.TryAuthenticateUser(username, out user))
			{
				Command command = user.Commands.Where(selectedCommand => selectedCommand.Name == commandName).FirstOrDefault();
				if (command != null)
				{
					if (command.Rights.Any(right => right.RightType == CommandRightType.Read))
					{
						mayUserRead = true;
					}
				}
			}

			commandUser = user;
			return isUserAuthenticated && mayUserRead;
		}

		public bool MayUserExecuteCommand(string username, string commandName, out User commandUser)
		{
			bool isUserAuthenticated = false;
			bool mayUserExecute = false;
			User user;
			if (isUserAuthenticated = _userAuthenticationProvider.TryAuthenticateUser(username, out user))
			{
				Command command = user.Commands.Where(selectedCommand => selectedCommand.Name == commandName).FirstOrDefault();
				if (command != null)
				{
					if (command.Rights.Any(right => right.RightType == CommandRightType.Execute))
					{
						mayUserExecute = true;
					}
				}
			}

			commandUser = user;
			return isUserAuthenticated && mayUserExecute;
		}
	}
}
