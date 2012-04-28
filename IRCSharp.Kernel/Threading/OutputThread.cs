using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Threading
{
	public class OutputThread : IRCSharp.Kernel.Threading.Base.Thread
	{
		private System.IO.TextWriter _textWriter = null;
		private string _output = String.Empty;

		public OutputThread(System.IO.TextWriter textWriter, string output) : base("output_thread")
		{
			_textWriter = textWriter;
			_output = output;
		}

		public override void Task()
		{
			base.Task();
			_textWriter.WriteLine(_output);
			_textWriter.Flush();
		}
	}
}
