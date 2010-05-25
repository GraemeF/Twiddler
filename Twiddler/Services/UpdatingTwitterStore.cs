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
        private readonly ITweetSource _source;
        private readonly ITweetStore _store;

        public UpdatingTwitterStore(ITweetSource source, ITweetStore store)
        {
            _source = source;
            _store = store;

            _source.Tweets.Subscribe(NewTweet);
        }

        #region IUpdatingTweetStore Members

        public IObservable<TweetId> NewTweets
        {
            get { return _newTweets; }
        }

        public void AddTweet(Tweet tweet)
        {
            _store.AddTweet(tweet);
        }

        public Tweet GetTweet(TweetId id)
        {
            return _store.GetTweet(id);
        }

        #endregion

        private void NewTweet(Tweet tweet)
        {
            _store.AddTweet(tweet);
            _newTweets.OnNext(tweet.Id);
        }
    }
}