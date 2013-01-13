using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserdefinedCommandExample
{
	[IRCSharp.Kernel.UserdefinedCommand("hello")]
	public class UserdefinedCommandExample : IRCSharp.Kernel.UserdefinedCommandBase
	{
		public override IRCSharp.Kernel.Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery query)
		{
			//You could use the authenticationProvider to either protect certain users from reading or writing. The default implementation is an XML based user and command list
			//where you can configure user and command authentication and permissions:
			IRCSharp.Kernel.Security.User userWrite;
			base.authenticationProvider.MayUserWriteToCommand(query.From, "hello", out userWrite);
			{

			}

			IRCSharp.Kernel.Security.User userRead;
			base.authenticationProvider.MayUserReadFromCommand(query.From, "hello", out userRead);
			{

			}

			//Execute rights is handled by the system, and can not yet be overruled.

			//This is how you get the parameters from the query
			Console.WriteLine("hello from command: " + query.CommandName);
			foreach (string param in query.Parameters)
			{
				Console.WriteLine("param in command: " + param);
			}
			return null;
		}

		public override void Init()
		{
			//here we can run code that should only run once, when the command is initiated. This happens when the bot is starting up.
		}
	}
}
