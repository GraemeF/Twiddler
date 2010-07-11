using System.Collections.Generic;
using System.Linq;
using Caliburn.Core.IoC;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Services
{
    [PerRequest(typeof (INewTweetFilter))]
    public class NewTweetFilter : INewTweetFilter
    {
        private readonly ITweetResolver _tweetResolver;

        public NewTweetFilter(ITweetStore tweetResolver)
        {
            _tweetResolver = tweetResolver;
        }

        #region INewTweetFilter Members

        public IEnumerable<Tweet> RemoveKnownTweets(IEnumerable<Tweet> tweets)
        {
            return tweets.Where(x => _tweetResolver.GetTweet(x.Id) == null);
        }

        #endregion
    }
}