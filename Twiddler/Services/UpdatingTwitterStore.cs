using System;
using System.Collections.Generic;
using Caliburn.Core.IoC;
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
        }

        #region IUpdatingTweetStore Members

        public IObservable<TweetId> Tweets
        {
            get { return _tweets; }
        }

        public void AddTweet(Tweet tweet)
        {
            _store.AddTweet(tweet);
            _tweets.OnNext(tweet.Id);
        }

        public Tweet GetTweet(TweetId id)
        {
            return _store.GetTweet(id);
        }

        #endregion
    }
}