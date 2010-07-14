using System.ComponentModel.Composition;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [Export(typeof (ITweetRequester))]
    public class UserTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public UserTimelineTweetRequester(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterUserTimeline request = Client.
                MakeRequestFor().
                Statuses().
                OnUserTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}