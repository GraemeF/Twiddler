using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using Raven.Client;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.TwitterStore.Interfaces;

namespace Twiddler.TwitterStore
{
    [Singleton(typeof (ITweetStore))]
    public class TwitterDocumentStore : ITweetStore
    {
        private readonly IDocumentStore _documentStore;

        private readonly object _mutex = new object();

        public TwitterDocumentStore(IDocumentStoreFactory documentStoreFactory)
        {
            _documentStore = documentStoreFactory.CreateDocumentStore();
        }

        #region ITweetStore Members

        public void Add(Tweet tweet)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    var existingEntry = session.Load<Tweet>(tweet.Id);

                    if (existingEntry == null)
                    {
                        session.Store(tweet);
                        session.SaveChanges();
                        Updated(this, EventArgs.Empty);
                    }
                }
        }

        public void Add(IEnumerable<Tweet> tweets)
        {
            lock (_mutex)
            {
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    foreach (Tweet tweet in tweets)
                        session.Store(tweet);

                    session.SaveChanges();
                }
                Updated(this, EventArgs.Empty);
            }
        }

        public Tweet GetTweet(string id)
        {
            lock (_mutex)
                using (IDocumentSession session = _documentStore.OpenSession())
                {
                    return session.Load<Tweet>(id);
                }
        }

        public IEnumerable<Tweet> GetInboxTweets()
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                return session.
                    Query<Tweet>("TweetsByIsArchived").
                    Where(x => !x.IsArchived).
                    ToList();
            }
        }

        public event EventHandler<EventArgs> Updated = delegate { };

        #endregion
    }
}