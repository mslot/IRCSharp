using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace IRCSharp.MSMQ
{
	public class MSMQWriter<T> : MSMQBase<T>
	{
		public MSMQWriter(string queueName)
			: base(queueName)
		{
		}

		public bool CanWrite
		{
			get
			{
				bool canWrite = false;
				if (MessageQueue != null)
				{
					canWrite = MessageQueue.CanWrite;
				}

				return canWrite;
			}
		}

		public void SendMessage(T data)
		{
			try
			{
				base.MessageQueue.Send(data);
			}
			catch (System.Messaging.MessageQueueException messageQueueException)
			{
				throw;
			}
			finally
			{
				base.MessageQueue.Close();
			}
		}
	}
}
