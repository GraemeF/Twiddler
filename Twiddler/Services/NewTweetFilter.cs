using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Twiddler.Core.Models;
using Twiddler.Core.Services;

namespace Twiddler.Services
{
    [Export(typeof (INewTweetFilter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NewTweetFilter : INewTweetFilter
    {
        private readonly ITweetResolver _tweetResolver;

        [ImportingConstructor]
        public NewTweetFilter(ITweetStore tweetResolver)
        {
            _tweetResolver = tweetResolver;
        }

        #region INewTweetFilter Members

        public IEnumerable<ITweet> RemoveKnownTweets(IEnumerable<ITweet> tweets)
        {
            return tweets.Where(x => _tweetResolver.GetTweet(x.Id) == null);
        }

        #endregion
    }
}