using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("Friends Timeline", typeof (ITweetRequester))]
    public class FriendsTimelineTweetRequester : TweetRequester
    {
        public FriendsTimelineTweetRequester(ITwitterClient client,
                                             IRequestLimitStatus requestLimitStatus,
                                             Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterFriendsTimeline request = Client.
                MakeRequestFor().
                Statuses().
                OnFriendsTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}