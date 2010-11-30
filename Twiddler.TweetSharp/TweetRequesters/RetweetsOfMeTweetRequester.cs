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
        public RetweetsOfMeTweetRequester(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override IEnumerable<TwitterStatus> GetStatuses(long since)
        {
            return since > 0L
                       ? Client.Service.ListRetweetsOfMyTweetsSince(since)
                       : Client.Service.ListRetweetsOfMyTweets();
        }
    }
}