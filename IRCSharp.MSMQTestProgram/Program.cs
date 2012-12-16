using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.MSMQTestProgram
{
	class Program
	{
		private static IRCSharp.Kernel.Messaging.MessageClient.MessageClient _client = new Kernel.Messaging.MessageClient.MessageClient("testProgram");

		static void Main(string[] args)
		{
			_client.ReceiveCompleted += ReceiveCompleted;
			_client.Start();
			Console.WriteLine("Press key to stop...");
			Console.ReadKey();
			_client.Stop();
			Console.WriteLine("stopped");
		}

		static void ReceiveCompleted(Kernel.Model.Query.IRCCommandQuery query)
		{
			Console.WriteLine(":::::::::::::::::::::::::::::::::::::::Received");
			Console.WriteLine(query.RawLine);

			if (query.RawLine.Contains("gonggong"))
			{
				IRCSharp.Kernel.Model.Query.IRCCommandQuery toBot = null;
				if (IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(query.Network,"PRIVMSG #mslot.dk :kingkong", out toBot))
				{
					Console.WriteLine("Trying to write to bot..." + query.RawLine);
					_client.WriteToBot(toBot);
				}
				else
				{
					Console.WriteLine("failed");
				}
			}
			else
			{
				Console.WriteLine("no gonggong");
			}
		}
	}
}
