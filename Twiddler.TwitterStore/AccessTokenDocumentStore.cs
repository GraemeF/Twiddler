using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.TwitterStore.Interfaces;

namespace Twiddler.TwitterStore
{
    [Singleton(typeof (IAccessTokenStore))]
    [Export(typeof (IAccessTokenStore))]
    public class AccessTokenDocumentStore : IAccessTokenStore
    {
        private readonly IDocumentStore _documentStore;

        private readonly object _mutex = new object();

        [ImportingConstructor]
        public AccessTokenDocumentStore(IDocumentStoreFactory documentStoreFactory)
        {
            _documentStore = documentStoreFactory.GetDocumentStore();
        }

        #region IAccessTokenStore Members

        public void Save(IAccessToken accessToken)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    session.Store(accessToken);
                    session.SaveChanges();
                }
        }

        public IAccessToken Load(string id)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    return session.Load<AccessToken>(id)
                           ?? new AccessToken(id, null, null);
                }
        }

        #endregion
    }
}