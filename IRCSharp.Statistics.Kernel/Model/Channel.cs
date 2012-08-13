using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Model
{
	public class Channel : IEquatable<Channel>
	{
		public List<IRCSharp.Kernel.Model.Query.IRCCommandQuery> Queries = new List<IRCSharp.Kernel.Model.Query.IRCCommandQuery>();
		public string ChannelName { get; private set; }
		public string Network { get; private set; }
		public string FullPath { get { return Network + "/" + ChannelName; } }

		public Channel(string channelName)
		{
			ChannelName = channelName;
		}

		public override bool Equals(object obj)
		{
			return this.Equals((Channel)obj);
		}

		public bool Equals(Channel other)
		{
			return this.FullPath.Equals(other.FullPath); //TODO: this could be done in a more proper manner... Or could it?
		}

		public override int GetHashCode()
		{
			return FullPath.GetHashCode();
		}
	}
}
