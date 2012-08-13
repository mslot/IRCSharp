using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Model
{
	public class User
	{
		public List<Channel> Channels = new List<Channel>();

		public User()
		{

		}

		public void AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			bool found = false;

			foreach (Channel channel in Channels)
			{
				if (channel.ChannelName == query.To)
				{
					channel.Queries.Add(query);
					found = true;
					break;
				}
			}

			if (!found)
			{
				var newChannel = new Channel(channelName: query.To);
				newChannel.Queries.Add(query);
				Channels.Add(newChannel);
			}
		}
	}
}
