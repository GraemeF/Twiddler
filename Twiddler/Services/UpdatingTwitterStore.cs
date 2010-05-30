using System;
using System.Collections.Generic;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Model;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (IUpdatingTweetStore))]
    public class UpdatingTwitterStore : IUpdatingTweetStore
    {
        private readonly IRequestConductor _requestConductor;
        private readonly ITweetStore _store;
        private readonly Subject<TweetId> _tweets = new Subject<TweetId>();

        public UpdatingTwitterStore(IRequestConductor requestConductor, ITweetStore store)
        {
            _requestConductor = requestConductor;
            _store = store;

            _requestConductor.Start(this);
        }

        #region IUpdatingTweetStore Members

        public IObservable<TweetId> Tweets
        {
            get { return _tweets; }
        }

        public bool AddTweet(TwitterStatus tweet)
        {
            if (_store.AddTweet(tweet))
            {
                _tweets.OnNext(tweet.GetTweetId());
                return true;
            }
            return false;
        }

        public TwitterStatus GetTweet(TweetId id)
        {
            return _store.GetTweet(id);
        }

        #endregion
    }
}