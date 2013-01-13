using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Security
{
	public class User
	{
		public bool IsAuthenticated { get; set; }
		public string Username { get; private set; }
		public ICollection<Command> Commands { get; private set; }

		public User(bool isAuthenticated, string username, ICollection<Command> commands)
		{
			Username = username;
			IsAuthenticated = isAuthenticated;
			Commands = commands;
		}

	}
}
