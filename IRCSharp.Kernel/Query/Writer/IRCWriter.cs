using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Query.Writer
{
	public class IRCWriter<TStream> where TStream : System.IO.Stream
	{
		private System.IO.TextWriter _clientWriter;

		public IRCWriter(TStream stream)
		{
			_clientWriter = System.IO.StreamWriter.Synchronized(new System.IO.StreamWriter(stream));
		}

		public void Join(string username, string hostname, string server, string name)
		{
			_clientWriter.WriteLine(String.Format("USER {0} {1} {2} :{3}", username, hostname, server, name));
			this.Flush();
		}

		private void Flush()
		{
			_clientWriter.Flush();
		}
	}
}
