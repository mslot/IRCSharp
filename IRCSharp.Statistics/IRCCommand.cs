//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace IRCSharp.Statistics
//{
//    [IRCSharp.Kernel.IRCCommand(IRCSharp.Kernel.Model.Query.ResponseCommand.ALL)]
//    public class StatisticsIRCCommand : IRCSharp.Kernel.ResponseCommandBase
//    {
//        private static Raven.Client.Embedded.EmbeddableDocumentStore _documentStore = new Raven.Client.Embedded.EmbeddableDocumentStore { DataDirectory = @"C:\data\testRavenDb", UseEmbeddedHttpServer = true };
//        private Raven.Client.IDocumentSession _session;

//        public StatisticsIRCCommand()
//        {
//        }

//        public override IRCSharp.Kernel.Model.Query.IRCCommandQuery Execute(IRCSharp.Kernel.Model.Query.IRCCommandQuery query)
//        {
//            _documentStore.Initialize();
//            using (_session = _documentStore.OpenSession())
//            {
//                _session.Store(query);
//                _session.SaveChanges();
//            }

//            Console.WriteLine("Collecting stats from: " + query.RawLine);

//            return null;
//        }


//        public override void Init()
//        {
//            _documentStore.Initialize();
//            _session = _documentStore.OpenSession();
//        }
//    }
//}
