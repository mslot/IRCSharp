using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace IRCSharp.MSMQ
{
	public class MSMQReader<T> : MSMQBase<T>
	{
		public event ReceiveCompletedHandler<T> ReceivedCompleted;

		public MSMQReader(string queueName) : base(queueName)
		{

		}

		public void BeginReceive()
		{
			base.MessageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
			base.MessageQueue.BeginReceive();
		}

		private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
		{
			MessageQueue messageQueueReceived = (MessageQueue)sender;
			T message = (T)messageQueueReceived.EndReceive(e.AsyncResult).Body;

			OnReceivedCompleted(message);

			messageQueueReceived.BeginReceive();
		}

		private void OnReceivedCompleted(T message)
		{
			if (ReceivedCompleted != null)
			{
				ReceivedCompleted(message);
			}
		}
	}
}
