using System.Collections.Concurrent;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Model;
using Twiddler.Core.Models;
using Twiddler.Core.Services;
using Twiddler.Models;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    //[Singleton(typeof (ITweetStore))]
    public class MemoryTweetStore : ITweetStore
    {
        private readonly ConcurrentDictionary<TweetId, TwitterStatus> _tweets = new ConcurrentDictionary<TweetId, TwitterStatus>();

        #region ITweetStore Members

        public bool AddTweet(TwitterStatus tweet)
        {
            return _tweets.TryAdd(tweet.GetTweetId(), tweet);
        }

        public TwitterStatus GetTweet(TweetId id)
        {
            return _tweets[id];
        }

        #endregion
    }
}