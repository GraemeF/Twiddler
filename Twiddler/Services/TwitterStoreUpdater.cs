using System;
using System.Collections.Generic;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ISelfUpdatingTweetStore))]
    public class TwitterStoreUpdater : ISelfUpdatingTweetStore
    {
        private readonly Subject<Tweet> _inboxTweets = new Subject<Tweet>();
        private readonly IRequestConductor _requestConductor;
        private readonly ITweetResolver _store;

        public TwitterStoreUpdater(IRequestConductor requestConductor, ITweetResolver store)
        {
            _requestConductor = requestConductor;
            _store = store;

            _requestConductor.Start(this);
        }

        #region ISelfUpdatingTweetStore Members

        public void AddTweet(Tweet tweet)
        {
            _store.AddTweet(tweet);
        }

        public Tweet GetTweet(string id)
        {
            return _store.GetTweet(id);
        }

        #endregion
    }
}