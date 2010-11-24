using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Home", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class HomeTimelineTweetRequestBuilder : TweetRequestBuilder
    {
        [ImportingConstructor]
        public HomeTimelineTweetRequestBuilder(ITwitterClient client,
                                               IRequestLimitStatus requestLimitStatus,
                                               Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequest CreateRequest(long since)
        {
            return Client.CreateRequestForStatusesOnHomeTimeline(since);
        }
    }
}