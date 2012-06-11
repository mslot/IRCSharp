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

		protected MSMQBase(string queuName)
		{
			QueueName = queuName;
			if (!System.Messaging.MessageQueue.Exists(QueueName))
			{
				MessageQueue = System.Messaging.MessageQueue.Create(QueueName);
			}
			else
			{
				MessageQueue = new System.Messaging.MessageQueue(QueueName);
			}

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
			MessageQueue.Dispose();
			this.Close();
		}
	}
}
