using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Home", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
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