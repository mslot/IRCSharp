using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Kernel.Bot
{
	public delegate void QueryParsedHandler(IRCSharp.Kernel.Query.IRCCommandQuery query);
	public delegate void IncomingHandler(string line);

	public class IRCBot
	{
		public event QueryParsedHandler QueryParsed;
		public event IncomingHandler Incoming;

		private System.IO.Stream _clientStream = null;
		private System.Net.Sockets.TcpClient _client = null;
		private System.IO.TextWriter _clientWriter = null;
		private System.IO.StreamReader _clientReader = null;
		private IRCSharp.Kernel.Collecters.CommandCollecter _commandCollecter = null;
		private int _port;
		private string _server;
		private string _dllPath;
		private string _username;
		private string _name;
		private string _hostname;
		private string _channels;

		private void OnQueryParsed(IRCSharp.Kernel.Query.IRCCommandQuery query)
		{
			if (QueryParsed != null)
			{
				QueryParsed(query);
			}
		}

		private void OnIncoming(string message)
		{
			if (Incoming != null)
			{
				Incoming(message);
			}
		}

		public IRCBot(string server, int port, string dllPath, string username, string name, string channels, string hostname = "rubber_duck_robert_bot@hell.org")
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
		}

		public void Start()
		{
			_commandCollecter.Start();
			JoinServer();
			JoinChannels();
			StartListning();
			Stop();
		}

		private void JoinServer()
		{
			_clientWriter.WriteLine(String.Format("USER {0} {1} {2} :{3}", _username, _hostname, _server, _name));
			_clientWriter.Flush();
			_clientWriter.WriteLine(String.Format("Nick {0}", _username));
			_clientWriter.Flush();
		}

		private void JoinChannels()
		{
			string line = null;
			bool run = true;
			while (run && (line = _clientReader.ReadLine()) != null)
			{
				OnIncoming(line);
				Query.IRCCommandQuery query = new Query.IRCCommandQuery(line);
				if (Parser.IRC.IRCQueryParser.TryParse(line, out query))
				{
					var incomingThread = new IRCSharp.Kernel.Threading.IncomingThread(query, _commandCollecter.CommandManager, _clientWriter);
					incomingThread.Start();
					if (query.Command == Query.ResponseCommand.RPL_ENDOFMOTD || query.Command == Query.ResponseCommand.ERR_NOMOTD)
					{
						OnQueryParsed(query);
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
			bool run = true;
			while (run)
			{
				string line = null;
				while (run && (line = _clientReader.ReadLine()) != null)
				{
					OnIncoming(line);
					Query.IRCCommandQuery query = new Query.IRCCommandQuery(line);
					if (Parser.IRC.IRCQueryParser.TryParse(line, out query))
					{
						OnQueryParsed(query);
						var incomingThread = new IRCSharp.Kernel.Threading.IncomingThread(query, _commandCollecter.CommandManager, _clientWriter);
						incomingThread.Start();
					}
					else
					{
						//error has happened
					}
				}
			}

			Stop();
		}

		private void Stop()
		{
			_commandCollecter.Stop();
			_client.Close();
			_clientStream.Close();
			_clientReader.Close();
			_clientWriter.Close();
		}
	}
}
