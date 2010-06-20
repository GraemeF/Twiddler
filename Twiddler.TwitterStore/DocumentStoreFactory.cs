using System;
using System.IO;
using System.Linq;
using Caliburn.Core.IoC;
using Raven.Client;
using Raven.Client.Document;
using Twiddler.Core.Models;
using Twiddler.TwitterStore.Interfaces;

namespace Twiddler.TwitterStore
{
    [Singleton(typeof (IDocumentStoreFactory))]
    public class DocumentStoreFactory : IDocumentStoreFactory
    {
        #region IDocumentStoreFactory Members

        public IDocumentStore CreateDocumentStore()
        {
            //var documentStore = new DocumentStore {DataDirectory = GetDataDirectory()};
            var documentStore = new DocumentStore {Url = "http://localhost:8080"};

            documentStore.Initialize();
            CreateIndices(documentStore);

            return documentStore;
        }

        #endregion

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

        private static string GetDataDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Twiddler");
        }
    }
}