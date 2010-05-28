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
        private readonly Subject<TweetId> _newTweets = new Subject<TweetId>();
        private readonly IRequestConductor _requestConductor;
        private readonly ITweetStore _store;
        private readonly Subject<Tweet> _tweets = new Subject<Tweet>();

        public UpdatingTwitterStore(IRequestConductor requestConductor, ITweetStore store)
        {
            _requestConductor = requestConductor;
            _store = store;
        }

        #region IUpdatingTweetStore Members

        public IObservable<TweetId> NewTweets
        {
            get { return _newTweets; }
        }

        public IObservable<Tweet> Tweets
        {
            get { return _tweets; }
        }

        public void AddTweet(Tweet tweet)
        {
            _store.AddTweet(tweet);
            _newTweets.OnNext(tweet.Id);
            _tweets.OnNext(tweet);
        }

        public Tweet GetTweet(TweetId id)
        {
            return _store.GetTweet(id);
        }

        #endregion
    }
}