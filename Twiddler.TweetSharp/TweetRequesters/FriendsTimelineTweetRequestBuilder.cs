using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Friends", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class FriendsTimelineTweetRequestBuilder : TweetRequestBuilder
    {
        [ImportingConstructor]
        public FriendsTimelineTweetRequestBuilder(ITwitterClient client,
                                                  IRequestLimitStatus requestLimitStatus,
                                                  Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequest CreateRequest(long since)
        {
            return Client.CreateRequestForStatusesOnFriendsTimeline(since);
        }
    }
}