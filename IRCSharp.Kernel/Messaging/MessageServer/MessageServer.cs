using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Messaging.MessageServer
{
	public class MessageServer<USend>
	{
		private MSMQ.MSMQServer<string, USend> _msmqServer = null;
		private ICollection<string> _messageQueues = new LinkedList<string>();

		public MessageServer(string receiveMessageQueueName)
		{
			_msmqServer = new MSMQ.MSMQServer<string, USend>(receiveMessageQueueName);
			_msmqServer.ReceiveCompleted += new MSMQ.ReceiveCompletedHandler<string>(ReceiveCompleted);
		}

		public void WriteMessage(USend data)
		{
			List<string> queuesToRemove = new List<string>();
			foreach (string queue in _messageQueues)
			{
				if (System.Messaging.MessageQueue.Exists(queue))
				{
					_msmqServer.WriteData(data, queue);
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
			_msmqServer.Start();
		}

		public void Stop()
		{
			_msmqServer.Stop();
		}

		private void ReceiveCompleted(string data)
		{
			if (!_messageQueues.Contains(data))
			{
				_messageQueues.Add(data);
			}
		}
	}
}
