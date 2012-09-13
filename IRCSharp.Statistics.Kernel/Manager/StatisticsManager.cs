using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Manager
{
	public class StatisticsManager
	{
		private Dal.IDal _databaseDal = null;

		public StatisticsManager()
		{
		}

		public void AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			_databaseDal.AddQuery(query);
		}
	}
}
