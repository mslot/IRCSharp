using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Model.Query
{
	/*
	 * IRC commands. This is either:
	 *	1. A common command like PRIVMSG
	 *	2. A RPL (reply)
	 *	3. An ERR (error)
	 *	
	 * The RPL or ERR is numeric.
	 * 
	 * TODO: Break this up in command, and reply commands.
	 */
	public enum IRCCommand
	{
		NOT_VALID_COMMAND_TYPE,
		PRIVMSG,
		PING,
		JOIN,
		RPL_ISUPPORT,
		RPL_UMODEIS,
		RPL_MOTDSTART,
		RPL_MOTD,
		RPL_ENDOFMOTD,
		ERR_NOMOTD,
		ALL //Represents all IRC commands
	}
}
