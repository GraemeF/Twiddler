using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetRequesters
{
    [Singleton("Home", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class HomeTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public HomeTimelineTweetRequester(ITwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Core.Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterHomeTimeline request = Client.
                MakeRequestFor().
                Statuses().
                OnHomeTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}