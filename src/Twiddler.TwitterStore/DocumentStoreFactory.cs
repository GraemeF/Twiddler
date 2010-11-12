﻿using System;
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
        private readonly Lazy<DocumentStore> _documentStore;
        private string _storeDirectory;

        public DocumentStoreFactory(StartupEventArgs args)
        {
            _storeDirectory = GetDefaultDataDirectory();
            ParseOptions(args);

            _documentStore = new Lazy<DocumentStore>(CreateDocumentStore);
        }

        #region IDocumentStoreFactory Members

        public IDocumentStore GetDocumentStore()
        {
            return _documentStore.Value;
        }

        #endregion

        private DocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore {DataDirectory = _storeDirectory};

            store.Initialize();
            CreateIndices(store);

            return store;
        }

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