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
		private Model.Query.Writer.IRCWriter<System.IO.Stream> _ircWriter = null;
		private System.IO.TextReader _clientReader = null;
		private Messaging.MessageServer.MessageServer<Model.Query.IRCCommandQuery> _messageServer = null;
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
			_ircWriter = new Model.Query.Writer.IRCWriter<System.IO.Stream>(_clientStream);
			_clientReader = new System.IO.StreamReader(_clientStream);
			_messageServer = new Messaging.MessageServer.MessageServer<Model.Query.IRCCommandQuery>(
				Messaging.Configuration.MessageServerConfiguration.BotServerQueuePath, 
				Messaging.Configuration.MessageServerConfiguration.BotServerOutgoingPath
				);

			_messageServer.OutgoingReveived += OutgoingReveived;
		}

		void OutgoingReveived(Model.Query.IRCCommandQuery query)
		{
			(new IRCSharp.Kernel.Threading.OutputThread(_ircWriter, query)).Start();
		}

		public override void Task()
		{
			StartBot();
		}

		public void StartBot()
		{
			_commandCollecter.Start();
			_messageServer.Start();
			JoinServer();
			JoinChannels();
			StartListning();
		}

		public void StopBot()
		{
			_ircWriter.Quit("goodbye");
		}

		private void JoinServer()
		{
			_ircWriter.User(_username, _hostname, _server, _name);
			_ircWriter.Nick(_username);
		}

		private void JoinChannels()
		{
			bool run = true;
			string line = null;
			while (run && (line = _clientReader.ReadLine()) != null)
			{
				Console.WriteLine(line);
				Model.Query.IRCCommandQuery query = new Model.Query.IRCCommandQuery(line);
				if (Parser.IRC.IRCQueryParser.TryParse(line, out query))
				{
					var incomingThread = new IRCSharp.Kernel.Threading.IncomingThread(query, _commandCollecter.CommandManager, _ircWriter);
					incomingThread.Start();
					if (query.Command == Model.Query.ResponseCommand.RPL_ENDOFMOTD || query.Command == Model.Query.ResponseCommand.ERR_NOMOTD)
					{
						_ircWriter.Join(_channels);
						run = false;
					}
				}
				else
				{
					//TODO error has happened. Needs to find a proper way of handling this.
				}
			}
		}

		private void StartListning()
		{
			string line = null;
			while ((line = _clientReader.ReadLine()) != null)
			{
				Console.WriteLine(line);
				Model.Query.IRCCommandQuery query = new Model.Query.IRCCommandQuery(line);
				if (Parser.IRC.IRCQueryParser.TryParse(line, out query))
				{
					var incomingThread = new IRCSharp.Kernel.Threading.IncomingThread(query, _commandCollecter.CommandManager, _ircWriter);
					_messageServer.WriteMessageToConnectors(query);
					incomingThread.Start();
				}
				else
				{
					//TODO error has happened. Needs to find a proper way of handling this.
				}
			}

			_messageServer.Stop();
			_commandCollecter.Stop();
			_client.Close();
			_clientStream.Close();
			_clientReader.Close();
			_ircWriter.Close();
		}
	}
}
