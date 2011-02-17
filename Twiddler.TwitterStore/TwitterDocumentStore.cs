namespace Twiddler.TwitterStore
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using Raven.Client;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.TwitterStore.Interfaces;
    using Twiddler.TwitterStore.Models;

    #endregion

    public class TwitterDocumentStore : ITweetStore
    {
        private readonly IDocumentStore _documentStore;

        private readonly object _mutex = new object();

        public TwitterDocumentStore(IDocumentStoreFactory documentStoreFactory)
        {
            _documentStore = documentStoreFactory.GetDocumentStore();
        }

        #region ITweetResolver members

        public ITweet GetTweet(string id)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                    return session.Load<Tweet>(id);
        }

        #endregion

        #region ITweetSink members

        public void Add(ITweet tweet)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    session.Store(tweet);
                    session.SaveChanges();
                }
        }

        public void Add(IEnumerable<ITweet> tweets)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    foreach (Tweet tweet in tweets)
                        session.Store(tweet);

                    session.SaveChanges();
                }
        }

        #endregion

        #region ITweetStore members

        public IEnumerable<ITweet> GetInboxTweets()
        {
            using (IDocumentSession session = _documentStore.OpenSession())
                return session.
                    Query<Tweet>("TweetsByIsArchived").
                    Where(x => !x.IsArchived).
                    ToList();
        }

        #endregion
    }
}