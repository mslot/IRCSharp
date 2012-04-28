using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	/*
	 * TODO clean up this mess. There are to much state held in this parser.
	 */
	public class IRCQueryParser
	{
		public IParsable CurrentState { get; private set; }
		public String Line { get; private set; }
		public Query Query { get; set; }
		public ParserStatus ParserStatus { get; private set; }
		private static IRCQueryParser _instance = null;

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

		private IRCQueryParser()
		{
		}

		public static bool TryParse(string line, out Query query)
		{
			_instance = new IRCQueryParser();
			_instance.Line = line;
			_instance.Query = new Query(line);
			_instance.Query = query = _instance.Parse();
			bool isLineParseable = _instance.IsParsable;

			return isLineParseable;
		}

		private Query Parse()
		{
			SetStartState();
			while (!(ParserStatus = CurrentState.Parse()).IsError && !ParserStatus.Done) ;

			//if (ParserStatus.IsError)
			//{
			//    throw ParserStatus.Exception ?? new Exception("something unexpected happened in the state.");
			//}

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
