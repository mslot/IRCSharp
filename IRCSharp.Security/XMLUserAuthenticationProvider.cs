using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public class XMLUserAuthenticationProvider : UserAuthenticationProviderBase //TODO: CRITIC: open/close of stream should be handled properly
	{
		private System.Xml.Linq.XDocument _documentFromStream = null;
		public XMLUserAuthenticationProvider(System.IO.StreamReader xmlStream)
		{
			_documentFromStream = LoadDocumentFromStream(xmlStream);
		}

		private System.Xml.Linq.XDocument LoadDocumentFromStream(System.IO.StreamReader xmlStream)
		{
			System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(xmlStream);
			return document;
		}

		public override User GetUser(string userPrefix)
		{
			User user = null;
			bool isAuthenticated = false;

			var userXmlNode = _documentFromStream.Descendants("user") 
				.Where(userElement => (string)userElement.Attribute("prefix") == userPrefix)
				.FirstOrDefault();

			if (userXmlNode != null)
			{
				string isAuthenticatedAttribute = (string)userXmlNode.Attribute("allow");
				string username = (string)userXmlNode.Attribute("prefix");
				isAuthenticated = Boolean.Parse(isAuthenticatedAttribute);
				ICollection<Command> commands = LoadCommandsFromUserNode(userXmlNode);
				user = new User(isAuthenticated, username, commands);
			}

			return user;
		}

		private ICollection<Command> LoadCommandsFromUserNode(System.Xml.Linq.XElement userXmlNode)
		{
			LinkedList<Command> commands = new LinkedList<Command>();
			var commandsNodes = userXmlNode.Descendants("commands");

			foreach (var commandsNode in commandsNodes)
			{
				foreach (var commandNode in commandsNode.Descendants("command"))
				{
					string name = (string)commandNode.Attribute("name");
					string right = (string)commandNode.Attribute("right");
					Command command = new Command(name, right);
					commands.AddLast(command);
				}
			}

			return commands;
		}
	}
}
