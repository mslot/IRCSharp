using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	public class IRCQueryParser
	{
		public IRCParserContext Context { get; private set; }

		private IRCQueryParser()
		{
			Context = new IRCParserContext();
			Context.CurrentState = new PrefixParser(Context);
		}

		public static bool TryParse(string network, string line, out Model.Query.IRCCommandQuery query) //TODO: remake method call. Network should not be part of this. Wrap this up in some context that holds network and line.
		{
			var instance = new IRCQueryParser();
			instance.Context.Line = line;
			instance.Context.Query = new Model.Query.IRCCommandQuery(line, network);
			instance.Context.Query = query = instance.Parse();
			bool errorProcessing = (instance.Context.CharCount != -1);
			bool errorReachingEnd = (instance.Context.CharCount == instance.Context.Line.Length);

			return errorProcessing && errorReachingEnd;
		}

		private Model.Query.IRCCommandQuery Parse()
		{
			while ((Context.CharCount != -1) && ((Context.CharCount = Context.CurrentState.Parse()) != Context.Line.Length)) ;

			return Context.Query;
		}
	}
}
