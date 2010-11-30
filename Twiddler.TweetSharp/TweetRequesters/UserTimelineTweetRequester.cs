using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Core.IoC;
using TweetSharp;
using Twiddler.Services.Interfaces;

namespace Twiddler.TweetSharp.TweetRequesters
{
    [Singleton("Mine", typeof (ITweetRequester))]
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

        protected override IEnumerable<TwitterStatus> GetStatuses(long since)
        {
            return since > 0L
                       ? Client.Service.ListTweetsOnHomeTimelineSince(since)
                       : Client.Service.ListTweetsOnHomeTimeline();
        }
    }
}