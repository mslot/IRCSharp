using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Messaging.MessageClient
{
	public class MessageClient
	{
		private MSMQ.MSMQServer<Query.IRCCommandQuery, string> _msmqServer = null;
		private string _name;
		private string _receiveQueuePath;

		public MessageClient(string name)
		{
			_name = name;
			_receiveQueuePath = String.Format(@".\private$\{0}", _name);
			_msmqServer = new MSMQ.MSMQServer<Query.IRCCommandQuery, string>(_receiveQueuePath);
		}

		public void Start()
		{
			_msmqServer.ReceiveCompleted += ReceiveCompleted;
			_msmqServer.Start();
			Signup();
		}

		private void ReceiveCompleted(Query.IRCCommandQuery data)
		{
			if (data != null)
			{
				Console.WriteLine(data.RawLine);
			}
		}

		public void Stop()
		{
			_msmqServer.Stop();
		}

		public void Signup()
		{
			_msmqServer.WriteData(_receiveQueuePath, Configuration.MessageServerConfiguration.BotServerQueuePath);
		}
	}
}
