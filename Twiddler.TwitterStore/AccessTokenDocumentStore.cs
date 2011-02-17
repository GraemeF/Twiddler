namespace Twiddler.TwitterStore
{
    #region Using Directives

    using Raven.Client;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.TwitterStore.Interfaces;

    #endregion

    public class AccessTokenDocumentStore : IAccessTokenStore
    {
        private readonly IDocumentStore _documentStore;

        private readonly object _mutex = new object();

        public AccessTokenDocumentStore(IDocumentStoreFactory documentStoreFactory)
        {
            _documentStore = documentStoreFactory.GetDocumentStore();
        }

        #region IAccessTokenStore members

        public AccessToken Load(string id)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                    return session.Load<AccessToken>(id)
                           ?? new AccessToken(id, null, null);
        }

        public void Save(AccessToken accessToken)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    session.Store(accessToken);
                    session.SaveChanges();
                }
        }

        #endregion
    }
}