using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace IRCSharp.MSMQ
{
	public delegate void ReceiveCompletedHandler<T>(T data);

	public class MSMQServer<TReceive, USend> : IDisposable where TReceive : class
	{
		public MSMQReader<TReceive> Reader { get; private set; }
		public event ReceiveCompletedHandler<TReceive> ReceiveCompleted;
		public string QueueName { get; private set; }

		public MSMQServer(string readerQueueName)
		{
			QueueName = readerQueueName;
			Reader = new MSMQReader<TReceive>(QueueName);
		}

		public void Start()
		{
			if (!System.Messaging.MessageQueue.Exists(QueueName))
			{
				System.Messaging.MessageQueue.Create(QueueName);
			}

			Reader.ReceivedCompleted += OnReceiveCompleted;
			Reader.BeginReceive();
			Debug.WriteLine("ended thread");

		}

		public void WriteData(USend data, string queueName)
		{
			using (var writer = new MSMQWriter<USend>(queueName))
			{
				if (writer.CanWrite)
				{
					writer.SendMessage(data);
				}
			}
		}

		public void OnReceiveCompleted(TReceive data)
		{
			if (ReceiveCompleted != null)
			{
				ReceiveCompleted(data);
			}
		}

		public void CloseQueue()
		{
			Reader.Close();
			System.Messaging.MessageQueue.Delete(QueueName);
		}

		public void Stop()
		{
			CloseQueue();
		}

		public void Dispose()
		{
			Stop();
		}
	}
}
