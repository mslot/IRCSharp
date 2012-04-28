using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	public enum CommandType
	{
		NOT_VALID_COMMAND_TYPE,	
		PASS,
		NICK,
		USER,
		SERVER,
		OPER,
		SQUIT,
		JOIN,
		PART,
		MODE,
		TOPIC,
		NAMES,
		LIST,
		INVITE,
		KICK,
		VERSION,
		STATS,
		LINKS,
		TIME,
		TRACE,
		ADMIN,
		PRIVMSG,
		NOTICE,
		WHOIS,
		WHOWAS,
		KILL,
		PING,
		PONG,
		ERROR
	}
}
