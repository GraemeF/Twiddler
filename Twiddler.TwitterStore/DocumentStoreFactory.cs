using System;
using System.IO;
using Caliburn.Core.IoC;
using Raven.Client;
using Raven.Client.Document;
using Twiddler.TwitterStore.Interfaces;

namespace Twiddler.TwitterStore
{
    [Singleton(typeof(IDocumentStoreFactory))]
    public class DocumentStoreFactory : IDocumentStoreFactory
    {
        public IDocumentStore CreateDocumentStore()
        {
            var documentStore = new DocumentStore { DataDirectory = GetDataDirectory() };

            documentStore.Initialize();

            return documentStore;
        }

        private static string GetDataDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Twiddler");
        }
    }
}