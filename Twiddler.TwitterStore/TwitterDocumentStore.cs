using System.Globalization;
using Caliburn.Core.IoC;
using Raven.Client;
using TweetSharp.Twitter.Model;
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

        public bool AddTweet(TwitterStatus tweet)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                session.Load<TwitterStatus>(tweet.Id.ToString(CultureInfo.InvariantCulture));

                session.Store(tweet);
            }
            return true;
        }

        public TwitterStatus GetTweet(TweetId id)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session.Load<TwitterStatus>(id.ToString());
            }
        }

        #endregion
    }
}