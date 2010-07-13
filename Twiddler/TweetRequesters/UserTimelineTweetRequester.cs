using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("User Timeline", typeof (ITweetRequester))]
    public class UserTimelineTweetRequester : TweetRequester
    {
        public UserTimelineTweetRequester(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterUserTimeline request = Client.
                MakeRequestFor().
                Statuses().
                OnUserTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}