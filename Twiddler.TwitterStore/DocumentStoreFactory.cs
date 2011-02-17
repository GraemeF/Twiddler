namespace Twiddler.TwitterStore
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;

    using NDesk.Options;

    using Raven.Client;
    using Raven.Client.Client;
    using Raven.Client.Document;
    using Raven.Client.Indexes;

    using Twiddler.TwitterStore.Interfaces;
    using Twiddler.TwitterStore.Models;

    #endregion

    public class DocumentStoreFactory : IDocumentStoreFactory
    {
        private readonly Lazy<DocumentStore> _documentStore;

        private bool _inMemory;

        private string _storeDirectory;

        public DocumentStoreFactory(StartupEventArgs args)
        {
            _storeDirectory = GetDefaultDataDirectory();
            ParseOptions(args);

            _documentStore = new Lazy<DocumentStore>(CreateDocumentStore);
        }

        #region IDocumentStoreFactory members

        public IDocumentStore GetDocumentStore()
        {
            return _documentStore.Value;
        }

        #endregion

        private static string GetDefaultDataDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Twiddler");
        }

        private DocumentStore CreateDocumentStore()
        {
            var store = new EmbeddableDocumentStore
                            {
                                RunInMemory = _inMemory
                            };

            if (!_inMemory &&
                _storeDirectory != null)
                store.DataDirectory = _storeDirectory;

            store.Initialize();
            CreateIndices(store);

            return store;
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
                                                   select new
                                                              {
                                                                  doc.IsArchived
                                                              }
                                 });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void ParseOptions(StartupEventArgs args)
        {
            var p = new OptionSet
                        {
                            { "store=", "the path of the RavenDB store.", v => _storeDirectory = v }, 
                            { "inMemory", "if set, use an in-memory RavenDB store.", v => RunInMemory() }
                        };

            p.Parse(args.Args);
        }

        private void RunInMemory()
        {
            _inMemory = true;
            _storeDirectory = null;
        }
    }
}