using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel
{
	public abstract class UserdefinedCommandBase : ICommand<string, IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery>
	{
		public abstract Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery query);
		protected IRCSharp.Kernel.Security.IAuthenticationProvider authenticationProvider;
		public string Location { get; set; }

		public UserdefinedCommandBase()
		{
			string xmlUserPath = String.Empty; //TODO: Add app.config path
			IRCSharp.Kernel.Security.IUserAuthenticationProvider userProvider = new IRCSharp.Kernel.Security.XMLUserAuthenticationProvider(
				new System.IO.StreamReader(
					new System.IO.MemoryStream(
						System.Text.Encoding.UTF8.GetBytes(xmlUserPath)
					)
				)
			);

			IRCSharp.Kernel.Security.ICommandAuthenticationProvider commandProvider = new IRCSharp.Kernel.Security.XMLCommandAuthenticationProvider(
				userProvider
			);

			authenticationProvider = new IRCSharp.Kernel.Security.AuthenticationProvider(commandProvider, userProvider);
		}

		public abstract void Init();
	}
}
