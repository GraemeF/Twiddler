using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.TwitterStore.Interfaces;

namespace Twiddler.TwitterStore
{
    [Singleton(typeof (ICredentialsStore))]
    [Export(typeof (ICredentialsStore))]
    public class CredentialsDocumentStore : ICredentialsStore
    {
        private readonly IDocumentStore _documentStore;

        private readonly object _mutex = new object();

        [ImportingConstructor]
        public CredentialsDocumentStore(IDocumentStoreFactory documentStoreFactory)
        {
            _documentStore = documentStoreFactory.GetDocumentStore();
        }

        #region ICredentialsStore Members

        public void Save(ITwitterCredentials credentials)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    session.Store(credentials);
                    session.SaveChanges();
                }
        }

        public ITwitterCredentials Load(string id)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    return session.Load<TwitterCredentials>(id)
                           ?? new TwitterCredentials(id, null, null);
                }
        }

        #endregion
    }
}