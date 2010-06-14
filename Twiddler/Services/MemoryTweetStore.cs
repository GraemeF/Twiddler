using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Services
{
    //[Singleton(typeof (ITweetStore))]
    public class MemoryTweetStore : ITweetStore
    {
        private readonly ConcurrentDictionary<string, Tweet> _tweets = new ConcurrentDictionary<string, Tweet>();

        #region ITweetStore Members

        public bool AddTweet(Tweet tweet)
        {
            return _tweets.TryAdd(tweet.Id, tweet);
        }

        public Tweet GetTweet(string id)
        {
            return _tweets[id];
        }

        public IEnumerable<Tweet> GetInboxTweets()
        {
            return _tweets.Values.Where(x => !x.IsArchived);
        }

        #endregion
    }
}