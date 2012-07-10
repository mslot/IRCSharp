using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Model.Query.Writer
{
	public class IRCWriter<TStream> where TStream : System.IO.Stream
	{
		private System.IO.TextWriter _clientWriter;

		public IRCWriter(TStream stream)
		{
			_clientWriter = System.IO.StreamWriter.Synchronized(new System.IO.StreamWriter(stream));
		}

		public void User(string username, string hostname, string server, string name)
		{
			_clientWriter.WriteLine(String.Format("USER {0} {1} {2} :{3}", username, hostname, server, name));
			this.Flush();
		}

		public void Quit(string message)
		{
			_clientWriter.WriteLine(String.Format("QUIT :{0}", message));
			this.Flush();
		}

		public void Nick(string nick)
		{
			_clientWriter.WriteLine(String.Format("Nick {0}", nick));
			this.Flush();
		}

		public void Join(string channels)
		{
			_clientWriter.WriteLine(String.Format("JOIN {0}", channels));
			this.Flush();
		}

		public void WriteQuery(IRCCommandQuery query)
		{
			_clientWriter.WriteLine(query.ToString());
			this.Flush();
		}

		public void Close()
		{
			_clientWriter.Close();
		}

		private void Flush()
		{
			_clientWriter.Flush();
		}
	}
}
