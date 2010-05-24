using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ITweetStore))]
    public class MemoryTweetStore : ITweetStore
    {
        private readonly Dictionary<TweetId, Tweet> _tweets = new Dictionary<TweetId, Tweet>();

        public MemoryTweetStore()
        {
            AllTweets = new ObservableCollection<Tweet>();
        }

        #region ITweetStore Members

        public void AddTweet(Tweet tweet)
        {
            _tweets.Add(tweet.Id, tweet);
            AllTweets.Add(tweet);
        }

        public ObservableCollection<Tweet> AllTweets { get; private set; }

        public Tweet GetTweet(TweetId id)
        {
            return _tweets[id];
        }

        #endregion
    }
}