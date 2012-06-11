using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			string server = System.Configuration.ConfigurationManager.AppSettings["server"];
			int port = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["port"]);
			string dllPath = System.Configuration.ConfigurationManager.AppSettings["dllPath"];
			string username = System.Configuration.ConfigurationManager.AppSettings["username"];
			string name = System.Configuration.ConfigurationManager.AppSettings["name"];
			string channels = System.Configuration.ConfigurationManager.AppSettings["channels"];

			var bot = new IRCSharp.Kernel.Bot.IRCBot(server,port,dllPath, username,name, channels);
			bot.Start();
		}
	}
}
