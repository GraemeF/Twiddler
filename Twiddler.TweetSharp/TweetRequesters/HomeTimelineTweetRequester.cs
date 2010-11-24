using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Home", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class HomeTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public HomeTimelineTweetRequester(ITweetSharpTwitterClient client,
                                          IRequestLimitStatus requestLimitStatus,
                                          Factories.TweetFactory tweetFactory)
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