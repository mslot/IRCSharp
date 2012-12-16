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
		public string Network { get; set; }

		public Commandbase()
		{

		}

		public Commandbase(string from, string to, string network)
		{
			From = from;
			To = to;
			Network = network;
		}
	}
}
