using System.Collections.Concurrent;
using Caliburn.Core.IoC;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    //[Singleton(typeof (ITweetStore))]
    public class MemoryTweetStore : ITweetStore
    {
        private readonly ConcurrentDictionary<TweetId, Tweet> _tweets = new ConcurrentDictionary<TweetId, Tweet>();

        #region ITweetStore Members

        public bool AddTweet(Tweet tweet)
        {
            return _tweets.TryAdd(tweet.Id, tweet);
        }

        public Tweet GetTweet(TweetId id)
        {
            return _tweets[id];
        }

        #endregion
    }
}