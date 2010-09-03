using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Core.IoC;
using NDesk.Options;
using Raven.Client;
using Raven.Client.Document;
using Twiddler.TwitterStore.Interfaces;
using Twiddler.TwitterStore.Models;

namespace Twiddler.TwitterStore
{
    [Singleton(typeof (IDocumentStoreFactory))]
    [Export(typeof (IDocumentStoreFactory))]
    public class DocumentStoreFactory : IDocumentStoreFactory
    {
        private string _storeDirectory;

        public DocumentStoreFactory(StartupEventArgs args)
        {
            _storeDirectory = GetDefaultDataDirectory();
            ParseOptions(args);
        }

        #region IDocumentStoreFactory Members

        public IDocumentStore CreateDocumentStore()
        {
            var documentStore = new DocumentStore {DataDirectory = _storeDirectory};

            documentStore.Initialize();
            CreateIndices(documentStore);

            return documentStore;
        }

        #endregion

        private void ParseOptions(StartupEventArgs args)
        {
            var p = new OptionSet
                        {
                            {"store=", "the path of the RavenDB store.", v => _storeDirectory = v}
                        };

            p.Parse(args.Args);
        }

        private void CreateIndices(IDocumentStore documentStore)
        {
            try
            {
                documentStore.DatabaseCommands.
                    PutIndex("TweetsByIsArchived",
                             new IndexDefinition<Tweet>
                                 {
                                     Map = docs => from doc in docs
                                                   orderby doc.CreatedDate descending
                                                   select new {doc.IsArchived}
                                 });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static string GetDefaultDataDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Twiddler");
        }
    }
}