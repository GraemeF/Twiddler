using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Retweets", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class RetweetsOfMeTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public RetweetsOfMeTweetRequester(ITwitterClientFactory clientFactory,
                                          IRequestLimitStatus requestLimitStatus,
                                          Factories.TweetFactory tweetFactory)
            : base(clientFactory, requestLimitStatus, tweetFactory)
        {
        }

        protected override IEnumerable<TwitterStatus> GetStatuses(TwitterService service, long since)
        {
            return since > 0L
                       ? service.ListRetweetsOfMyTweetsSince(since)
                       : service.ListRetweetsOfMyTweets();
        }
    }
}