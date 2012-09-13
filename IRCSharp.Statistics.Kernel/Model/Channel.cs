using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Model
{
	public class Channel : IEquatable<Channel>
	{
		public List<IRCSharp.Kernel.Model.Query.IRCCommandQuery> Queries = new List<IRCSharp.Kernel.Model.Query.IRCCommandQuery>();
		public string Name { get; private set; }
		public string Network { get; private set; } //TODO: implement network
		public string FullPath { get { return Network + "/" + Name; } }

		public Channel(string channelName)
		{
			Name = channelName;
		}

		public override bool Equals(object obj)
		{
			if (obj is Channel)
			{
				return this.Equals((Channel)obj);
			}
			else
			{
				return false;
			}
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
