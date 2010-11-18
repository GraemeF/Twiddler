using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [Singleton("Home", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class HomeTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public HomeTimelineTweetRequester(ITwitterClient client,
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
                OnHomeTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}