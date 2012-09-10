using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Dal
{
	class SQLCEDal : IDal
	{
		private System.Data.SqlServerCe.SqlCeConnection _connection = null;
		
		public SQLCEDal(string connectionString)
		{
			_connection = new System.Data.SqlServerCe.SqlCeConnection(connectionString);
			_connection.Open();
		}

		public void AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			System.Data.SqlServerCe.SqlCeCommand command = _connection.CreateCommand();
			if (DoesUserExists(query))
			{
				if (DoesChannelExists(query))
				{
					//create query
				}
				else
				{
					//create channel
					//create query
				}
			}
			else
			{
				//create user
				//create channel
				//create query
			}

		}

		private bool DoesUserExists(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			throw new NotImplementedException();
		}

		private bool DoesChannelExists(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			throw new NotImplementedException();
		}

		public Model.User GetUser(string nick)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			_connection.Dispose();
		}
	}
}
