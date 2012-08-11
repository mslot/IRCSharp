using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Dal
{
	public interface IDal
	{
		void AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query);
	}
}
