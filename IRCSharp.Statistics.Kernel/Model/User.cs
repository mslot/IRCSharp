using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Model
{
	public class User : IComparable<User>
	{
		public List<Channel> Channels = new List<Channel>();
		public string Nick { get; private set; }
		public string Network { get; private set; }

		public User(string network, string nick)
		{
			Nick = nick;
			Network = network;
		}

		public void AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			bool found = false;

			foreach (Channel channel in Channels)
			{
				if (channel.Name == query.To) //TODO: is To an channel? Is it a comma seperated list of channels and users?
				{
					channel.Queries.Add(query);
					found = true;
					break;
				}
			}

			if (!found)
			{
				var newChannel = new Channel(channelName: query.To); //TODO: this could be a seperated list of users and channels. This should be handled! We should check of To is a channel!!
				newChannel.Queries.Add(query);
				Channels.Add(newChannel);
			}
		}

		public int CompareTo(User other)
		{
			string thisUser = Network + "/" + Nick;
			string otherUser = Network + "/" + Nick;

			return this.CompareTo(other);
		}
	}
}
