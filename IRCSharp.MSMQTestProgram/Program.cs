using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.MSMQTestProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			IRCSharp.Kernel.Messaging.MessageClient.MessageClient client = new Kernel.Messaging.MessageClient.MessageClient("testProgram");
			client.Start();
			Console.WriteLine("Press key to stop...");
			Console.ReadKey();
			client.Stop();
			Console.WriteLine("stopped");
		}
	}
}
