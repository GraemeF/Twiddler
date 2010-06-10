using Caliburn.Core.IoC;
using Raven.Client;
using Twiddler.Core.Services;
using Twiddler.Models;

namespace Twiddler.TwitterStore
{
    [Singleton(typeof (ITweetStore))]
    public class TwitterDocumentStore : ITweetStore
    {
        private readonly IDocumentStore _documentStore;

        public TwitterDocumentStore(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        #region ITweetStore Members

        public bool AddTweet(Tweet tweet)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                session.Load<Tweet>(tweet.Id);

                session.Store(tweet);
            }
            return true;
        }

        public Tweet GetTweet(string id)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session.Load<Tweet>(id);
            }
        }

        #endregion
    }
}