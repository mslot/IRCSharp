using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Model.Query
{
	public class Commandbase
	{
		public string From { get; set; }
		public string To { get; set; }

		public Commandbase()
		{

		}

		public Commandbase(string from, string to)
		{
			From = from;
			To = to;
		}
	}
}
