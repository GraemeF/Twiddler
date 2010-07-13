using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("Home Timeline", typeof (ITweetRequester))]
    public class HomeTimelineTweetRequester : TweetRequester
    {
        public HomeTimelineTweetRequester(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterHomeTimeline request = Client.
                MakeRequestFor().
                Statuses().
                OnHomeTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}