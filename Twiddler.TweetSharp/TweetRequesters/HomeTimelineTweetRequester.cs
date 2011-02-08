namespace Twiddler.TweetSharp.TweetRequesters
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using Caliburn.Core.IoC;

    using global::TweetSharp;

    using Twiddler.Services.Interfaces;

    #endregion

    [Singleton("Home", typeof(ITweetRequester))]
    [Export(typeof(ITweetRequester))]
    public class HomeTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public HomeTimelineTweetRequester(ITwitterClientFactory clientFactory, 
                                          IRequestLimitStatus requestLimitStatus, 
                                          Factories.TweetFactory tweetFactory)
            : base(clientFactory, requestLimitStatus, tweetFactory)
        {
        }

        protected override IEnumerable<TwitterStatus> GetStatuses(TwitterService service, long since)
        {
            return since > 0L
                       ? service.ListTweetsOnHomeTimelineSince(since)
                       : service.ListTweetsOnHomeTimeline();
        }
    }
}