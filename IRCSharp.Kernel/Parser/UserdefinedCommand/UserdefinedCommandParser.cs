using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.UserdefinedCommand
{
	/// <summary>
	/// Parses the userdefined command string.
	/// 
	/// TODO: refactor this in small fragments of parsers like IRCQueryParser.
	/// </summary>
	public class UserdefinedCommandParser
	{
		private UserdefinedCommandParser()
		{

		}


		public static bool TryParse(string network, string line, out Model.Query.UserdefinedCommandQuery query) //TODO: remake method call. Network should not be part of this. Wrap this up in some context that holds network and line.
		{
			UserdefinedCommandParser parser = new UserdefinedCommandParser();
			IRCSharp.Kernel.Model.Query.IRCCommandQuery ircQuery = null;
			Model.Query.UserdefinedCommandQuery output = null;
			bool parsedIRCQuery = IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(network, line, out ircQuery);
			bool parsed = false;

			if (ircQuery.Command == Model.Query.IRCCommand.PRIVMSG && IsUserdefinedCommand(ircQuery.Parameter))
			{
				string commandName = parser.ParseCommandName(ircQuery.Parameter);
				IList<string> commandNameParameters = parser.ParseCommandParameters(ircQuery.Parameter);
				query = new Model.Query.UserdefinedCommandQuery(commandName, ircQuery);
				query.AddParameters(commandNameParameters);
				parsed = true;

			}

			query = output;
			return parsed;
		}

		public static bool TryParse(IRCSharp.Kernel.Model.Query.IRCCommandQuery ircQuery, out IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery userdefinedCommandQuery)
		{
			UserdefinedCommandParser parser = new UserdefinedCommandParser();
			bool parsed = false;
			Model.Query.UserdefinedCommandQuery query = null;
			if (ircQuery.Command == Model.Query.IRCCommand.PRIVMSG && IsUserdefinedCommand(ircQuery.Parameter))
			{
				string commandName = parser.ParseCommandName(ircQuery.Parameter);
				IList<string> commandNameParameters = parser.ParseCommandParameters(ircQuery.Parameter);
				query = new Model.Query.UserdefinedCommandQuery(commandName, ircQuery);
				query.AddParameters(commandNameParameters);
				parsed = true;
			}

			userdefinedCommandQuery = query;
			return parsed;
		}

		private static bool IsUserdefinedCommand(string ircParameter)
		{
			int start = ircParameter.IndexOf(':');

			if (ircParameter.Substring(start + 1, 1) == "!")
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Parsing the parameter section of an IRC response command:
		/// </summary>
		/// <param name="ircQueryParameter">The parameter to the irc query, fx: #mslot.dk :!command arg1 arg2 arg3</param>
		/// <returns>the userdefined commandname</returns>
		private string ParseCommandName(string ircQueryParameter)
		{
			int start = ircQueryParameter.IndexOf(':') + 2;
			int to = ircQueryParameter.IndexOf(' ', start);

			if (to == -1) //there is no parameters eg. no whitespace
			{
				to = ircQueryParameter.Length;
			}

			return ircQueryParameter.Substring(start, to - start);
		}

		/// <summary>
		/// Parsers the parameters to the userdefined command
		/// </summary>
		/// <param name="ircQueryParameter">the parameter section of the irc query.</param>
		/// <returns>a list of command parameters.</returns>
		private IList<string> ParseCommandParameters(string ircQueryParameter)
		{
			int start = ircQueryParameter.IndexOf(':') + 1;
			int firstWhitespace = ircQueryParameter.IndexOf(' ', start);
			IList<string> parameters = new List<string>();

			if (firstWhitespace != -1) //if first whitespace is -1 then there is no parameters
			{
				foreach (string parameter in ircQueryParameter.Substring(firstWhitespace, ircQueryParameter.Length - firstWhitespace).Split(' '))
				{
					parameters.Add(parameter);
				}
			}

			return parameters;
		}
	}
}
