using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Messaging.MessageServer
{
	public delegate void OutgoingEventHandler(IRCSharp.Kernel.Model.Query.IRCCommandQuery query);

	public class MessageServer<USend>
	{
		private MSMQ.MSMQServer<string, USend> _connectionServer = null;
		private MSMQ.MSMQServer<IRCSharp.Kernel.Model.Query.IRCCommandQuery, IRCSharp.Kernel.Model.Query.IRCCommandQuery> _outgoingServer = null;
		private ICollection<string> _messageQueues = new LinkedList<string>();
		public event OutgoingEventHandler OutgoingReveived;

		public MessageServer(string connectorsQueueName, string outgoingQueueName)
		{
			_outgoingServer = new MSMQ.MSMQServer<IRCSharp.Kernel.Model.Query.IRCCommandQuery, IRCSharp.Kernel.Model.Query.IRCCommandQuery>(outgoingQueueName);
			_outgoingServer.ReceiveCompleted += OutgoingServerReceiveCompleted;

			_connectionServer = new MSMQ.MSMQServer<string, USend>(connectorsQueueName);
			_connectionServer.ReceiveCompleted += ConnectorsReceiveCompleted;
		}

		public void OnOutgingReceived(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			if (OutgoingReveived != null)
			{
				OutgoingReveived(query);
			}
		}

		public void WriteMessageToConnectors(USend data)
		{
			List<string> queuesToRemove = new List<string>();
			foreach (string queue in _messageQueues)
			{
				if (System.Messaging.MessageQueue.Exists(queue))
				{
					_connectionServer.WriteData(data, queue);
				}
				else
				{
					queuesToRemove.Add(queue);
				}
			}

			foreach (string queue in queuesToRemove)
			{
				_messageQueues.Remove(queue);
			}
		}

		public void Start()
		{
			_outgoingServer.Start();
			_connectionServer.Start();
		}

		public void Stop()
		{
			_outgoingServer.Stop();
			_connectionServer.Stop();
		}

		private void ConnectorsReceiveCompleted(string data)
		{
			if (!_messageQueues.Contains(data))
			{
				_messageQueues.Add(data);
			}
		}

		private void OutgoingServerReceiveCompleted(IRCSharp.Kernel.Model.Query.IRCCommandQuery data)
		{
			OnOutgingReceived(data);
		}
	}
}
