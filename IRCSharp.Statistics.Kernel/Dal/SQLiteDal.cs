using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Dal
{
	//TODO: Refactor this
	public class SQLiteDal : IDal
	{
		private System.Data.SQLite.SQLiteConnection _connection = null;
		
		public SQLiteDal(string connectionString)
		{
			_connection = new System.Data.SQLite.SQLiteConnection(connectionString);
			_connection.Open();
		}

		public Model.User GetUser(string network, string nick, int numberOfQueries)
		{
			Int64 id = -1;
			Model.User user = null;

			bool created = true;
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "select id from User where nick = @nick AND network = @network limit 1";
			command.Parameters.AddWithValue("@nick", nick);
			command.Parameters.AddWithValue("@network", network);
			try
			{
				using (var dataReader = command.ExecuteReader())
				{

					while (dataReader.Read())
					{
						id = dataReader.GetInt64(dataReader.GetOrdinal("id"));
					}
				}

				user = new Model.User(network, nick);
			}
			catch (Exception) //TODO propper exception handling needed here
			{
			}

			ICollection<IRCSharp.Kernel.Model.Query.IRCCommandQuery> queries = GetQueriesForUser(id, numberOfQueries);
			foreach (IRCSharp.Kernel.Model.Query.IRCCommandQuery query in queries)
			{
				user.AddQuery(query);
			}
			

			return user;
		}

		public void RemoveAllQueries()
		{
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "Delete from Query";
			command.ExecuteNonQuery();
		}

		public void RemoveAllChannels()
		{
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "Delete from Channel";
			command.ExecuteNonQuery();
		}

		public void RemoveUsers()
		{
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "Delete from User";
			command.ExecuteNonQuery();
		}

		private ICollection<IRCSharp.Kernel.Model.Query.IRCCommandQuery> GetQueriesForUser(long id, int numberOfQueries)
		{
			ICollection<IRCSharp.Kernel.Model.Query.IRCCommandQuery> queries = new LinkedList<IRCSharp.Kernel.Model.Query.IRCCommandQuery>();
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "select Channel.network, Channel.id, Query.rawQuery from Channel left join Query on Channel.id = Query.channelId where Channel.userId = @userId and Query.userId = @userId limit @limit";
			command.Parameters.AddWithValue("@userId", id);
			command.Parameters.AddWithValue("@limit", numberOfQueries);
			try
			{
				using (var dataReader = command.ExecuteReader())
				{

					while (dataReader.Read())
					{
						string rawQuery = dataReader.GetString(dataReader.GetOrdinal("rawQuery"));
						string network = dataReader.GetString(dataReader.GetOrdinal("network"));
						IRCSharp.Kernel.Model.Query.IRCCommandQuery query;

						if (IRCSharp.Kernel.Parser.IRC.IRCQueryParser.TryParse(network, rawQuery, out query))
						{
							queries.Add(query);
						}
					}
				}
			}
			catch (Exception) //TODO propper exception handling needed here
			{
			}

			return queries;
		}

		public bool AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			Int64 userId;
			Int64 channelId;
			bool addedQuery = false;

			if (TryGetUserId(query, out userId))
			{
				if (TryGetChannelId(query, out channelId, userId))
				{
					addedQuery = TryAddQueryToChannel(query, channelId, userId) || addedQuery;
				}
				else
				{
					addedQuery = TryCreateChannelToUser(query, userId, out channelId) || addedQuery;
					addedQuery = TryAddQueryToChannel(query, channelId, userId) || addedQuery;
				}
			}
			else
			{
				addedQuery = TryCreateUser(query, out userId) || addedQuery;
				addedQuery = TryCreateChannelToUser(query, userId, out channelId) || addedQuery;
				addedQuery = TryAddQueryToChannel(query, channelId, userId) || addedQuery;
			}

			return addedQuery;

		}

		public void Dispose()
		{
			_connection.Dispose();
		}



		private bool TryCreateUser(IRCSharp.Kernel.Model.Query.IRCCommandQuery query, out Int64 userId)
		{
			bool created = true;
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "insert into User (nick,network) values(@nick, @network); SELECT last_insert_rowid();";
			command.Parameters.AddWithValue("@nick", query.Nick);
			command.Parameters.AddWithValue("@network", query.Network);
			try
			{
				object obj = command.ExecuteScalar();
				userId = (Int64)obj;
			}
			catch (Exception) //TODO propper exception handling needed here
			{
				created = false;
				userId = default(int);
			}

			return created;
		}

		private bool TryCreateChannelToUser(IRCSharp.Kernel.Model.Query.IRCCommandQuery query, Int64 userId, out Int64 channelId)
		{
			bool created = true;

			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "insert into Channel (name, network, userId) values (@name, @network, @userId); SELECT last_insert_rowid();";
			command.Parameters.AddWithValue("@name", query.Channel);
			command.Parameters.AddWithValue("@network", query.Network);
			command.Parameters.AddWithValue("@userId", userId);

			try
			{
				channelId = (Int64)command.ExecuteScalar();
			}
			catch (Exception) //TODO propper exception handling needed here
			{
				created = false;
				channelId = default(int);
			}

			return created;
		}

		private bool TryAddQueryToChannel(IRCSharp.Kernel.Model.Query.IRCCommandQuery query, Int64 channelId, Int64 userId)
		{
			bool created = true;
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();
			command.CommandText = "insert into Query (rawQuery, channelId, userId) values(@rawQuery, @channelId, @userId); SELECT last_insert_rowid();";
			command.Parameters.AddWithValue("@rawQuery", query.RawLine);
			command.Parameters.AddWithValue("@channelId", channelId);
			command.Parameters.AddWithValue("@userId", userId);

			try
			{
				channelId = (Int64)command.ExecuteScalar();
			}
			catch (Exception) //TODO propper exception handling needed here
			{
				created = false;
			}

			return created;
		}

		private bool TryGetChannelId(IRCSharp.Kernel.Model.Query.IRCCommandQuery query, out Int64 channelId, Int64 userId)
		{
			bool foundChannel = false;
			channelId = -1;
			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();

			command.CommandText = "select id from Channel where name = @channelName and network = @network and userId = @userId";
			command.Parameters.AddWithValue("@channelName", query.Channel);
			command.Parameters.AddWithValue("@network", query.Network);
			command.Parameters.AddWithValue("@userId", userId);
			command.Prepare();
			System.Data.SQLite.SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				if (channelId != -1 && foundChannel) //means that there is more than one channel with channel name.  That is NOT possible on the same network!!
				{
					/*
					 * TODO: Create log entry, and get the first possible channelId
					 * Now, this is infact wrong to log statistics for two channels, on one entry, so the statistics is infact invalid.
					 */
					break;
				}

				channelId = reader.GetInt32(reader.GetOrdinal("id"));
				foundChannel = true;
			}
			reader.Close();

			return foundChannel;
		}

		private bool TryGetUserId(IRCSharp.Kernel.Model.Query.IRCCommandQuery query, out Int64 userId)
		{
			bool foundUser = false;
			userId = -1;

			System.Data.SQLite.SQLiteCommand command = _connection.CreateCommand();

			command.CommandText = "select id from User where nick = @nick and network = @network";
			command.Parameters.AddWithValue("@nick", query.Nick);
			command.Parameters.AddWithValue("@network", query.Network);
			command.Prepare();
			System.Data.SQLite.SQLiteDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				if (userId != -1 && foundUser) //means that there is more than one user with that nick. That is NOT possible on the same network!!
				{
					/*
					 * TODO: Create log entry, and get the first possible channelId
					 * Now, this is infact wrong to log statistics for two different users, on one entry, so the statistics is infact invalid.
					 */
					break;
				}

				userId = reader.GetInt32(reader.GetOrdinal("id"));
				foundUser = true;
			}
			reader.Close();

			return foundUser;
		}
	}
}
