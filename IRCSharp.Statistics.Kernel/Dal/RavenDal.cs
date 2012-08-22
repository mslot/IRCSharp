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
					//TODO: find channel
					bool doesChannelExist = _session.Query<Model.User>().Any(user => user.Nick == query.Nick && user.Channels.Any(channel => channel.ChannelName == query.Channel));
					if (doesChannelExist)
					{
						//TODO. With partial update -> add query to channel
						//_documentStore.DatabaseCommands.Patch("users/" + query.Nick, )
					}
					else
					{
						//if not exists, add channel, then add query, then add channel with partial update
					}
				}
				else
				{
					//TODO: add user to user
					//TODO: add channel with query
				}

				_session.SaveChanges();
			}

			Console.WriteLine("Collecting stats from: " + query.RawLine);
		}

		public void Dispose()
		{
			_documentStore.Dispose();
		}
	}
}
