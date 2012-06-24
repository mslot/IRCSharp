using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Messaging.MessageClient
{
	public delegate void ReceiveCompletedEventHandler(Query.IRCCommandQuery query);
	public class MessageClient
	{
		private MSMQ.MSMQServer<Query.IRCCommandQuery, string> _msmqServer = null;
		private string _name;
		private string _receiveQueuePath;

		public event ReceiveCompletedEventHandler ReceiveCompleted;

		public MessageClient(string name)
		{
			_name = name;
			_receiveQueuePath = String.Format(@".\private$\{0}", _name);
			_msmqServer = new MSMQ.MSMQServer<Query.IRCCommandQuery, string>(_receiveQueuePath);
		}

		public void Start()
		{
			_msmqServer.ReceiveCompleted += InternaReceiveCompleted;
			_msmqServer.Start();
			Signup();
		}

		public void OnReceiveCompleted(Query.IRCCommandQuery query)
		{
			if (ReceiveCompleted != null)
			{
				ReceiveCompleted(query);
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

		public void WriteToBot(Query.IRCCommandQuery query)
		{
			MSMQ.MSMQWriter<Query.IRCCommandQuery> writer = new MSMQ.MSMQWriter<Query.IRCCommandQuery>(Configuration.MessageServerConfiguration.BotServerOutgoingPath);
			writer.SendMessage(query);
			writer.Close();
		}

		private void InternaReceiveCompleted(Query.IRCCommandQuery data)
		{
			if (data != null)
			{
				OnReceiveCompleted(data);
			}
		}

	}
}
