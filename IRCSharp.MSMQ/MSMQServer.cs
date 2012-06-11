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

		public MSMQServer(string readerQueueName)
		{
			Reader = new MSMQReader<TReceive>(readerQueueName);
		}

		public override void Start()
		{
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
