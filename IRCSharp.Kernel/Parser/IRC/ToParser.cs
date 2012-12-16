using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Parser.IRC
{
	public class ToParser : IParsable
	{
		private IRCParserContext _context = null;

		public ToParser(IRCParserContext context)
		{
			_context = context;
		}

		public int Parse()
		{
			int nextCharCount = ParseToString(_context.Line);
			_context.CurrentState = new ParamsParser(_context);

			return nextCharCount;
		}

		private int ParseToString(string line)
		{
			int index = line.IndexOf(' ', _context.CharCount + 1);

			if (index == -1) //Handles queries like ":WiZ JOIN #Twilight_zone" with no spaces in the end ... Linkin Park ftw btw!!
				index = line.Length;

			if ((index - 1 - _context.CharCount) >= 0) //TODO: Clean up spaghetti code
			{
				string to = line.Substring(_context.CharCount + 1, index - 1 - _context.CharCount);

				if (to.StartsWith("#")) //TODO: this only handles one channel, not lists of channels. Make this better! And is it correct only to check after #? What about private messages?
				{
					_context.Query.To = to;
				}
				else
				{
					_context.Query.To = String.Empty;
				}
			}
			return line.IndexOf(' ', _context.CharCount);
		}
	}
}
