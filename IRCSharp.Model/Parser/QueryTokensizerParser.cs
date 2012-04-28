using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	class QueryTokensizerParser
	{
		public IParsable CurrentState { get; private set; }
		public String Line { get; private set; }
		public Query Query { get; private set; }
		public ParserStatus ParserStatus { get; private set; }

		public bool IsParsable
		{
			get
			{
				if (ParserStatus == null)
				{
					return false;
				}
				else
				{
					return !ParserStatus.IsError;
				}
			}
		}

		protected int CharCount
		{
			get
			{
				if (ParserStatus == null)
				{
					return 0;
				}
				else
				{
					return ParserStatus.CharCount;
				}
			}
		}

		private QueryParser _queryParser = null;

		public QueryTokensizerParser(QueryParser queryParser)
		{
			if (Query == null)
			{
				Query = new Query();
			}

			this._queryParser = queryParser;
			Line = queryParser.Line;
			SetStartState();
		}

		public Query Parse()
		{
			while (!(ParserStatus = CurrentState.Parse()).IsError && !ParserStatus.Done) ;

			if (ParserStatus.IsError)
			{
				throw ParserStatus.Exception ?? new Exception("something unexpected happened in the state.");
			}

			return Query;
		}

		internal void SetStartState()
		{
			CurrentState = new ParsePrefix(this);
		}

		internal void SetCommandParserState()
		{
			CurrentState = new ParseCommand(this);
		}

		internal void SetParseParamsState()
		{
			CurrentState = new ParseParams(this);
		}
	}
}
