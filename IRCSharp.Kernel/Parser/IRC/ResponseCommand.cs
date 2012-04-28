using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	/*
	 * Responses from server. This is either:
	 *	1. A common command like PRIVMSG
	 *	2. A RPL (reply)
	 *	3. An ERR (error)
	 *	
	 * The RPL or ERR is numeric.
	 */
	public enum ResponseCommand
	{
		NOT_VALID_RESPONSE_COMMAND_TYPE,
		PRIVMSG,
		PING,
		JOIN,
		RPL_ISUPPORT,
		RPL_UMODEIS,
		RPL_MOTDSTART,
		RPL_MOTD,
		RPL_ENDOFMOTD,
		ERR_NOMOTD
	}
}
