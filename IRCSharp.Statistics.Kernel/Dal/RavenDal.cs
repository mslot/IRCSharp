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

		public RavenDal(string dataDirectory, bool useEmbeddedHttpServer = false)
		{
			InitDatastore(dataDirectory, useEmbeddedHttpServer);
		}

		private void InitDatastore(string dataDirectory, bool useEmbeddedHttpServer)
		{
			_documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore { DataDirectory = dataDirectory, UseEmbeddedHttpServer = useEmbeddedHttpServer };
			_documentStore.Initialize();
			_session = _documentStore.OpenSession();

		}

		public void AddQuery(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
		{
			_documentStore.Initialize();
			using (_session = _documentStore.OpenSession())
			{
				_session.Store(query);
				_session.SaveChanges();
			}

			Console.WriteLine("Collecting stats from: " + query.RawLine);
		}
	}
}
