namespace Twiddler.Services
{
    #region Using Directives

    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Twiddler.Core.Models;
    using Twiddler.Core.Services;
    using Twiddler.TwitterStore.Models;

    #endregion

    // [Singleton(typeof (ITweetStore))]
    public class MemoryTweetStore : ITweetResolver
    {
        private readonly ConcurrentDictionary<string, ITweet> _tweets = new ConcurrentDictionary<string, ITweet>();

        public void Add(ITweet tweet)
        {
            _tweets.TryAdd(tweet.Id, tweet);
        }

        public void Add(IEnumerable<ITweet> tweets)
        {
            foreach (Tweet tweet in tweets)
                Add(tweet);
        }

        #region ITweetResolver members

        public ITweet GetTweet(string id)
        {
            return _tweets[id];
        }

        #endregion
    }
}