using System.Collections.Concurrent;
using System.Collections.Generic;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof (ITweetStore))]
    public class MemoryTweetStore : ITweetStore
    {
        private readonly ConcurrentDictionary<TweetId, Tweet> _tweets = new ConcurrentDictionary<TweetId, Tweet>();

        #region ITweetStore Members

        public void AddTweet(Tweet tweet)
        {
            _tweets[tweet.Id] = tweet;
        }

        public Tweet GetTweet(TweetId id)
        {
            return _tweets[id];
        }

        #endregion
    }
}