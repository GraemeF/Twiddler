using System.Collections.Concurrent;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Services
{
    //[Singleton(typeof (ITweetStore))]
    public class MemoryTweetStore : ITweetResolver
    {
        private readonly ConcurrentDictionary<string, Tweet> _tweets = new ConcurrentDictionary<string, Tweet>();

        #region ITweetResolver Members

        public void Add(Tweet tweet)
        {
            _tweets.TryAdd(tweet.Id, tweet);
        }

        public Tweet GetTweet(string id)
        {
            return _tweets[id];
        }

        #endregion
    }
}