using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Retweets", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class RetweetsOfMeTweetRequestBuilder : TweetRequestBuilder
    {
        [ImportingConstructor]
        public RetweetsOfMeTweetRequestBuilder(ITwitterClient client,
                                               IRequestLimitStatus requestLimitStatus,
                                               Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequest CreateRequest(long since)
        {
            return Client.CreateRequestForRetweetsOfMe(since);
        }
    }
}