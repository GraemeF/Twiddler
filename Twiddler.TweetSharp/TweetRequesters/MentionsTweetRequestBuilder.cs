using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core;
using Twiddler.Core.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [PerRequest("Mentions", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class MentionsTweetRequestBuilder : TweetRequestBuilder
    {
        [ImportingConstructor]
        public MentionsTweetRequestBuilder(ITwitterClient client,
                                           IRequestLimitStatus requestLimitStatus,
                                           Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequest CreateRequest(long since)
        {
            return Client.CreateRequestForMentions(since);
        }
    }
}