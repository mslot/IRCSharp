using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace IRCSharp.MSMQ
{
	public abstract class MSMQBase<T> : IDisposable
	{
		protected MessageQueue MessageQueue { get; private set; }
		public string QueueName { get; private set; }

		protected MSMQBase(string queueName)
		{
			QueueName = queueName;
			MessageQueue = new System.Messaging.MessageQueue(QueueName);
			MessageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
		}

		public void Close()
		{
			if (MessageQueue != null)
			{
				MessageQueue.Close();
				MessageQueue.Dispose();
			}
		}

		public void Dispose()
		{
			this.Close();
		}
	}
}
