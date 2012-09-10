using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCSharp.Statistics.Kernel.Dal
{
	public class RavenDal : IDal
	{
		private Raven.Client.Embedded.EmbeddableDocumentStore _documentStore = null;
		private Raven.Client.IDocumentSession _session;

		public RavenDal(string dataDirectory, bool useEmbeddedHttpServer = false, bool runInMemory = false)
		{
			InittializeDatastore(dataDirectory, useEmbeddedHttpServer, runInMemory);
		}

		private void InittializeDatastore(string dataDirectory, bool useEmbeddedHttpServer, bool runInMemory)
		{
			_documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore { DataDirectory = dataDirectory, UseEmbeddedHttpServer = useEmbeddedHttpServer, RunInMemory = runInMemory };
			_documentStore.Initialize();
		}

		public void AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			using (_session = _documentStore.OpenSession())
			{
				bool doesUserExist = _session.Query<Model.User>().Any(user => user.Nick == query.Nick);
				if (doesUserExist)
				{
					bool doesChannelExist = _session.Query<Model.User>().Any(user => user.Nick == query.Nick && user.Channels.Any(channel => channel.ChannelName == query.Channel));
					if (doesChannelExist)
					{
						//TODO: With partial update -> add query to channel
						Model.User user = GetUser(query.Nick);
						user.AddQuery(query);
						_session.Store(user);
					}
					else
					{
						Model.Channel newChannel = new Model.Channel(query.Channel);
						newChannel.Queries.Add(query);
						_documentStore.DatabaseCommands.Patch("users/" + query.Nick,
							new[] 
							{ 
								 new Raven.Abstractions.Data.PatchRequest
								 {
									Type = Raven.Abstractions.Data.PatchCommandType.Add,
									Name = "Channels",
									Value = Raven.Json.Linq.RavenJObject.FromObject(newChannel)
								 }
							});
					}
				}
				else
				{
					Model.User newUser = new Model.User(query.Nick);
					newUser.AddQuery(query);
					_session.Store(newUser);
				}

				_session.SaveChanges();
			}
		}

		public Model.User GetUser(string nick)
		{
			return _session.Load<Model.User>(nick);
		}

		public void Dispose()
		{
			_documentStore.Dispose();
		}
	}
}
