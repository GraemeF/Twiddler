namespace Twiddler.TweetSharp.TweetRequesters
{
    #region Using Directives

    using System.Collections.Generic;

    using global::TweetSharp;

    using Twiddler.Core.Models;

    #endregion

    public class RetweetsOfMeTweetRequester : TweetRequester
    {
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