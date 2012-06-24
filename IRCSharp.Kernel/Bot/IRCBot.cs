using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Bot
{
	public class IRCBot : IRCSharp.Threading.Base.Thread
	{
		private System.IO.Stream _clientStream = null;
		private System.Net.Sockets.TcpClient _client = null;
		private System.IO.TextWriter _clientWriter = null;
		private System.IO.TextReader _clientReader = null;
		private Messaging.MessageServer.MessageServer<Query.IRCCommandQuery> _messageServer = null;
		private IRCSharp.Kernel.Collecters.CommandCollecter _commandCollecter = null;
		private int _port;
		private string _server;
		private string _dllPath;
		private string _username;
		private string _name;
		private string _hostname;
		private string _channels;

		public IRCBot(string server, int port, string dllPath, string username, string name, string channels, string hostname = "rubber_duck_robert_bot@hell.org") : base("main_bot_thread")
		{
			_channels = channels;
			_username = username;
			_dllPath = dllPath;
			_name = name;
			_hostname = hostname;
			_server = server;
			_port = port;
			_commandCollecter = new IRCSharp.Kernel.Collecters.CommandCollecter(_dllPath);
			_client = new System.Net.Sockets.TcpClient();
			_client.Connect(_server, _port);
			_clientStream = _client.GetStream();
			_clientWriter = System.IO.StreamWriter.Synchronized(new System.IO.StreamWriter(_clientStream));
			_clientReader = new System.IO.StreamReader(_clientStream);
			_messageServer = new Messaging.MessageServer.MessageServer<Query.IRCCommandQuery>(Messaging.Configuration.MessageServerConfiguration.BotServerQueuePath);
		}

		public override void Task()
		{
			StartBot();
		}

		private void StartBot()
		{
			_commandCollecter.Start();
			_messageServer.Start();
			JoinServer();
			JoinChannels();
			StartListning();
		}

		public void Stop()
		{
			_clientWriter.WriteLine("QUIT :goodbye");
			_clientWriter.Flush();
		}

		private void JoinServer()
		{
			_clientWriter.WriteLine(String.Format("USER {0} {1} {2} :{3}", _username, _hostname, _server, _name)); //TODO maybe writer could be moved out in a class that wraps writing known commands
			_clientWriter.Flush();
			_clientWriter.WriteLine(String.Format("Nick {0}", _username));
			_clientWriter.Flush();
		}

		private void JoinChannels()
		{
			bool run = true;
			string line = null;
			while (run && (line = _clientReader.ReadLine()) != null)
			{
				Query.IRCCommandQuery query = new Query.IRCCommandQuery(line);
				if (Parser.IRC.IRCQueryParser.TryParse(line, out query))
				{
					var incomingThread = new IRCSharp.Kernel.Threading.IncomingThread(query, _commandCollecter.CommandManager, _clientWriter);
					incomingThread.Start();
					if (query.Command == Query.ResponseCommand.RPL_ENDOFMOTD || query.Command == Query.ResponseCommand.ERR_NOMOTD)
					{
						_clientWriter.WriteLine(String.Format("JOIN {0}", _channels));
						_clientWriter.Flush();
						run = false;
					}
				}
				else
				{
					//error has happened
				}
			}
		}

		private void StartListning()
		{
			string line = null;
			while ((line = _clientReader.ReadLine()) != null)
			{
				Query.IRCCommandQuery query = new Query.IRCCommandQuery(line);
				if (Parser.IRC.IRCQueryParser.TryParse(line, out query))
				{
					var incomingThread = new IRCSharp.Kernel.Threading.IncomingThread(query, _commandCollecter.CommandManager, _clientWriter);
					_messageServer.WriteMessage(query);
					incomingThread.Start();
				}
				else
				{
					//error has happened
				}
			}

			_messageServer.Stop();
			_commandCollecter.Stop();
			_client.Close();
			_clientStream.Close();
			_clientReader.Close();
			_clientWriter.Close();
		}
	}
}
