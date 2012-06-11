using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Query.Writer
{
	public class IRCWriter<TStream> where TStream : System.IO.Stream
	{
		private System.IO.TextWriter _stream;

		public IRCWriter(TStream stream)
		{
			_stream = System.IO.StreamWriter.Synchronized(new System.IO.StreamWriter(stream));
		}
	}
}
