using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [Singleton("Friends", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class FriendsTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public FriendsTimelineTweetRequester(ITwitterClient client,
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
                OnFriendsTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}