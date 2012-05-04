using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	/*
	 * TODO clean up this mess. There are to much state held in this parser. Move out to a Context behaps?
	 */
	public class IRCQueryParser
	{
		public IParsable CurrentState { get; private set; }
		public String Line { get; private set; }
		public Query.IRCCommandQuery Query { get; set; }
		public int CharCount { get; set; }

		private IRCQueryParser()
		{
		}

		public static bool TryParse(string line, out Query.IRCCommandQuery query)
		{
			var instance = new IRCQueryParser();
			instance.Line = line;
			instance.Query = new Query.IRCCommandQuery(line);
			instance.Query = query = instance.Parse();
			bool errorProcessing = (instance.CharCount != -1);
			bool errorReachingEnd = (instance.CharCount == instance.Line.Length);

			return errorProcessing && errorReachingEnd;
		}

		private Query.IRCCommandQuery Parse()
		{
			SetStartState();
			while ((CharCount != -1) && ((CharCount = CurrentState.Parse()) != Line.Length)) ;

			return Query;
		}

		internal void SetStartState()
		{
			CurrentState = new PrefixParser(this);
		}

		internal void SetCommandParserState()
		{
			CurrentState = new CommandParser(this);
		}

		internal void SetParseParamsState()
		{
			CurrentState = new ParamsParser(this);
		}
	}
}
