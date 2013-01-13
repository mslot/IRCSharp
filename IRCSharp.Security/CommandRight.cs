using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Security
{
	public enum CommandRightType
	{
		Unknown = 0,
		Read = 1,
		Write = 2,
		Execute = Read | Write
	}

	public class CommandRight
	{
		public CommandRightType RightType { get; private set; }

		public CommandRight(CommandRightType rightType)
		{
			RightType = rightType;
		}

		public CommandRight(string commandRight)
		{
			RightType = Convert(commandRight);
		}

		public static CommandRightType Convert(string right)
		{
			CommandRightType commandRightType = CommandRightType.Unknown;
			switch(right)
			{
				case "read":
					commandRightType = CommandRightType.Read;
					break;
					
				case "write":
					commandRightType = CommandRightType.Write;
					break;

				case "execute":
					commandRightType = CommandRightType.Execute;
					break;
			}

			return commandRightType;
		}
	}
}
