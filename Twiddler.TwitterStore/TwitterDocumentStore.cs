using Caliburn.Core.IoC;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

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
                var existingEntry = session.Load<Tweet>(tweet.Id);

                if (existingEntry == null)
                {
                    session.Store(tweet);
                    session.SaveChanges();
                    return true;
                }
            }
            return false;
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