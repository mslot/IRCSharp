using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Model.Parser
{
	internal class ParserStatus
	{
		public bool IsError { get; set; }
		public Exception Exception { get; set; }
		public string Message { get; set; }
		public string Name { get; set; }
		public int CharCount { get; set; }
		public bool Done { get; set; }

		public ParserStatus()
		{
			Done = false;
			IsError = false;
		}
	}
}
