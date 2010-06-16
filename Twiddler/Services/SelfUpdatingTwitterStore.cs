using System;
using System.Collections.Generic;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ISelfUpdatingTweetStore))]
    public class SelfUpdatingTwitterStore : ISelfUpdatingTweetStore
    {
        private readonly Subject<Tweet> _inboxTweets = new Subject<Tweet>();
        private readonly IRequestConductor _requestConductor;
        private readonly ITweetResolver _store;

        public SelfUpdatingTwitterStore(IRequestConductor requestConductor, ITweetResolver store)
        {
            _requestConductor = requestConductor;
            _store = store;

            _requestConductor.Start(this);
        }

        #region ISelfUpdatingTweetStore Members

        public IObservable<Tweet> InboxTweets
        {
            get { return _inboxTweets; }
        }

        public bool AddTweet(Tweet tweet)
        {
            if (_store.AddTweet(tweet))
            {
                _inboxTweets.OnNext(tweet);
                return true;
            }
            return false;
        }

        public Tweet GetTweet(string id)
        {
            return _store.GetTweet(id);
        }

        #endregion
    }
}