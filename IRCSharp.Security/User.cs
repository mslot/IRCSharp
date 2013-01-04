using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public class User
	{
		public string Username { get; private set; }
		public bool IsAuthenticated { get; set; }
		public User(bool isAuthenticated, string username)
		{
			Username = username;
			IsAuthenticated = isAuthenticated;
		}

	}
}
