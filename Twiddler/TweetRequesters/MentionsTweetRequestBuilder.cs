using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("Mentions", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class MentionsTweetRequestBuilder : TweetRequestBuilder
    {
        [ImportingConstructor]
        public MentionsTweetRequestBuilder(ITwitterClient client,
                                      IRequestLimitStatus requestLimitStatus,
                                      Core.Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterRequest CreateRequest(long since)
        {
            return Client.CreateRequestForMentions(since);
        }
    }
}