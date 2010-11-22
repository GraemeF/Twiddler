using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [Singleton("Mine", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class UserTimelineTweetRequestBuilder : TweetRequestBuilder
    {
        [ImportingConstructor]
        public UserTimelineTweetRequestBuilder(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Core.Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequest CreateRequest(long since)
        {
            return Client.CreateRequestForStatusesOnUserTimeline(since);
        }
    }
}