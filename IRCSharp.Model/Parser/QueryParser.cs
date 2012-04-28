using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	public class QueryParser
	{
		private static QueryParser _instance = null;
		public string Line { get; private set; }

		private QueryParser(string line)
		{
			Line = line;
		}

		public static bool TryParse(string line, out Query query)
		{
			_instance = new QueryParser(line);

			QueryTokensizerParser parser = new QueryTokensizerParser(_instance);
			query = parser.Parse();
			bool isLineParseable = parser.IsParsable;

			return isLineParseable;
		}
	}
}
