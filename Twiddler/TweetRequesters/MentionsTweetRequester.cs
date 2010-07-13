using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [PerRequest("Mentions", typeof (ITweetRequester))]
    public class MentionsTweetRequester : TweetRequester
    {
        public MentionsTweetRequester(ITwitterClient client,
                                      IRequestLimitStatus requestLimitStatus,
                                      Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterStatusMentions request = Client.
                MakeRequestFor().
                Statuses().
                Mentions();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}