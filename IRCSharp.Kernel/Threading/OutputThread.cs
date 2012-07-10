using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Threading
{
	public class OutputThread : IRCSharp.Threading.Base.Thread
	{
		private Model.Query.Writer.IRCWriter<System.IO.Stream> _ircWriter = null;
		private Model.Query.IRCCommandQuery _query = null;

		public OutputThread(Model.Query.Writer.IRCWriter<System.IO.Stream> ircWriter, Model.Query.IRCCommandQuery query)
			: base("output_thread")
		{
			_ircWriter = ircWriter;
			_query = query;
		}

		public override void Task()
		{
			base.Task();
			
			if (_query != null)
			{
				_ircWriter.WriteQuery(_query);
			}
		}
	}
}
