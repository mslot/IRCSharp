using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Security
{
	public class XMLUserAuthenticationProvider : UserAuthenticationProviderBase
	{
		private System.IO.StreamReader _xmlStream = null;

		public XMLUserAuthenticationProvider(System.IO.StreamReader xmlStream)
		{
			_xmlStream = xmlStream;
		}

		private System.Xml.Linq.XDocument LoadDocumentFromStream()
		{
			System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(_xmlStream);
			return document;
		}

		public override User GetUser(string userPrefix)
		{
			User user = null;
			bool isAuthenticated = false;
			System.Xml.Linq.XDocument documentFromStream = LoadDocumentFromStream();
			var userXmlNode = documentFromStream.Descendants("user")
				.Where(userElement => (string)userElement.Attribute("prefix") == userPrefix)
				.FirstOrDefault();

			if (userXmlNode != null)
			{
				string isAuthenticatedAttribute = (string)userXmlNode.Attribute("allow");
				string username = (string)userXmlNode.Attribute("prefix");
				isAuthenticated = Boolean.Parse(isAuthenticatedAttribute);
				user = new User(isAuthenticated, username);
			}


			return user;
		}
	}
}
