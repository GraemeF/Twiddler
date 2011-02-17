namespace Twiddler.TweetSharp.TweetRequesters
{
    #region Using Directives

    using System.Collections.Generic;

    using global::TweetSharp;

    using Twiddler.Services.Interfaces;

    #endregion

    public class FriendsTimelineTweetRequester : TweetRequester
    {
        public FriendsTimelineTweetRequester(ITwitterClientFactory clientFactory, 
                                             IRequestLimitStatus requestLimitStatus, 
                                             Factories.TweetFactory tweetFactory)
            : base(clientFactory, requestLimitStatus, tweetFactory)
        {
        }

        protected override IEnumerable<TwitterStatus> GetStatuses(TwitterService service, long since)
        {
            return since > 0L
                       ? service.ListTweetsOnFriendsTimelineSince(since)
                       : service.ListTweetsOnFriendsTimeline();
        }
    }
}