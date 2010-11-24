using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp.Twitter.Fluent;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Friends", typeof (ITweetRequester))]
    [Export(typeof (ITweetRequester))]
    public class FriendsTimelineTweetRequester : TweetRequester
    {
        [ImportingConstructor]
        public FriendsTimelineTweetRequester(ITweetSharpTwitterClient client,
                                             IRequestLimitStatus requestLimitStatus,
                                             Factories.TweetFactory tweetFactory)
            : base(client, requestLimitStatus, tweetFactory)
        {
        }

        protected override ITwitterLeafNode CreateRequest(long since)
        {
            ITwitterFriendsTimeline request = Client.
                MakeRequestFor().
                Statuses().
                OnFriendsTimeline();
            return since > 0L
                       ? request.Since(since)
                       : request;
        }
    }
}