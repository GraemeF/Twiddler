using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [Singleton("Mine", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class UserTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public UserTimelineTweetRequester(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Core.Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequestBuilder CreateRequest(long since)
        {
            ITwitterRequestBuilder request = Client.
                MakeRequestFor().
                Statuses().
                OnUserTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}