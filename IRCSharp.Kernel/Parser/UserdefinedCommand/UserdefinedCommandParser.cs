using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.UserdefinedCommand
{
	public class UserdefinedCommandParser
	{
		private UserdefinedCommandParser()
		{

		}

		public static bool TryParse(IRCSharp.Kernel.Model.Query.IRCCommandQuery ircQuery, out IRCSharp.Kernel.Model.Query.UserdefinedCommandQuery userdefinedCommandQuery)
		{
			UserdefinedCommandParser parser = new UserdefinedCommandParser();
			bool parsed = false;
			Model.Query.UserdefinedCommandQuery query = null;
			if (ircQuery.Command == Model.Query.ResponseCommand.PRIVMSG && IsUserdefinedCommand(ircQuery.Parameter))
			{
				string to = parser.ParseTo(ircQuery.Parameter);
				string from = parser.ParseFrom(ircQuery.Prefix);
				string commandName = parser.ParseCommandName(ircQuery.Parameter);
				IList<string> commandNameParameters = parser.ParseCommandParameters(ircQuery.Parameter);
				query = new Model.Query.UserdefinedCommandQuery(to, from, commandName, ircQuery);
				query.AddParameters(commandNameParameters);
				parsed = true;
			}

			userdefinedCommandQuery = query;
			return parsed;
		}

		private static bool IsUserdefinedCommand(string ircParameter)
		{
			int start = ircParameter.IndexOf(':');

			if (ircParameter.Substring(start+1, 1) == "!")
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// parsing the parameter to find the receiver.
		/// TODO: This should be parsed by the IRC parser
		/// </summary>
		/// <param name="parameter">the parameter of the irc query, fx  #mslot.dk :!command arg1 arg2 arg3</param>
		/// <returns>the receiver(s) of the irc query.</returns>
		private string ParseTo(string parameter)
		{
			int start = 0;
			int length = parameter.IndexOf(':')-1;

			string to = parameter.Substring(start, length);

			return to;
		}

		/// <summary>
		/// Parsing the prefix to find whom sent the message.
		/// TODO: This should be parsed by the IRC parser
		/// </summary>
		/// <param name="prefix">The prefix of the irc query, fx: :mslot!~mslot@56344eba.rev.stofanet.dk</param>
		/// <returns>the sender</returns>
		private string ParseFrom(string prefix)
		{
			int start = 0;
			int length = prefix.IndexOf('!');

			return prefix.Substring(start, length);
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
