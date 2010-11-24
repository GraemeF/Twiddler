using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Mine", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class UserTimelineTweetRequestBuilder : TweetRequestBuilder
    {
        [ImportingConstructor]
        public UserTimelineTweetRequestBuilder(ITwitterClient client,
                                               IRequestLimitStatus requestLimitStatus,
                                               Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequest CreateRequest(long since)
        {
            return Client.CreateRequestForStatusesOnUserTimeline(since);
        }
    }
}