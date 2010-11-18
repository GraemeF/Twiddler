using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using Twiddler.Core.Services;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("Mentions", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class MentionsTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public MentionsTweetRequester(ITwitterClient client,
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
                Mentions();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}